using Microsoft.Extensions.Logging;
using Simplicity.Database;
using Simplicity.Infrastructure;
using Simplicity.WebApi;
using System.Text.Json;

namespace Simplicity.BusinessLogic
{
    public class RocketLaunchingLogic : IRocketLaunchingLogic
    {
        private IThrustCalculator thrustCalculator;
        private IRocketDatabaseRetriever rocketDatabaseRetriever;
        private IRocketQueuePoller rocketQueuePoller;
        private IRocketLaunchingService rocketLaunchingService;
        private ILogger<RocketLaunchingLogic> logger;
        private ICoordinateCalculator coordinateCalculator;
        private IAsyncDelay asyncDelay;

        public RocketLaunchingLogic(IThrustCalculator thrustCalculator, IRocketDatabaseRetriever rocketDatabaseRetriever, IRocketQueuePoller rocketQueuePoller, IRocketLaunchingService rocketLaunchingService, ILogger<RocketLaunchingLogic> logger, ICoordinateCalculator coordinateCalculator, IAsyncDelay asyncDelay)
        {
            this.thrustCalculator = thrustCalculator ?? throw new ArgumentNullException(nameof(thrustCalculator));
            this.rocketDatabaseRetriever = rocketDatabaseRetriever ?? throw new ArgumentNullException(nameof(rocketDatabaseRetriever));
            this.rocketQueuePoller = rocketQueuePoller ?? throw new ArgumentNullException(nameof(rocketQueuePoller));
            this.rocketLaunchingService = rocketLaunchingService ?? throw new ArgumentNullException(nameof(rocketLaunchingService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.coordinateCalculator = coordinateCalculator ?? throw new ArgumentNullException(nameof(coordinateCalculator));
            this.asyncDelay = asyncDelay ?? throw new ArgumentNullException(nameof(asyncDelay));
        }

        public async Task TryToLaunchARocket()
        {
            var rocketLaunchMessage = await rocketQueuePoller.PollForRocketNeedingLaunch();
            if (rocketLaunchMessage == null)
            {
                await asyncDelay.DelayAsync(TimeSpan.FromSeconds(5));
                return;
            }

            try
            {
                var databaseResult = await rocketDatabaseRetriever.FindRocketStatistics(rocketLaunchMessage.RocketModelId);
                var requiredThrust = thrustCalculator.CalculateThrust(
                    databaseResult.ThrustValue1,
                    databaseResult.ThrustValue2,
                    databaseResult.ThrustValue3,
                    databaseResult.ThrustValue4,
                    databaseResult.ThrustValue5,
                    rocketLaunchMessage.numberOfSlothsToLaunch
                );

                var foodForJourney = await rocketDatabaseRetriever.GetFoodToFeedSlothsOnTheirJourney(rocketLaunchMessage.RocketModelId, rocketLaunchMessage.numberOfSlothsToLaunch);
                if (foodForJourney == null)
                {
                    if (rocketLaunchMessage.numberOfSlothsToLaunch < 10)
                    {
                        foodForJourney = new FoodForJourney { NumberOfCourses = 1, Foods = new[] { "only slightly toxic leaves" } };
                    }
                    else
                    {
                        throw new Exception("We can't cater a trip for 10 or more sloths without carefully planned food for space travel!");
                    }
                }

                var coordinatesToLandOnMoon = coordinateCalculator.CalculateCoordinatesToLand(rocketLaunchMessage.numberOfSlothsToLaunch, foodForJourney);

                var rocketLaunchResult = await rocketLaunchingService.LaunchRocket(rocketLaunchMessage.RocketModelId, rocketLaunchMessage.numberOfSlothsToLaunch, requiredThrust, foodForJourney);

                if (rocketLaunchResult.launchWasSuccessful)
                {
                    await rocketQueuePoller.RemoveMessageFromQueue(rocketLaunchMessage.messageId);
                }
                else
                {
                    throw new Exception($"Failed to launch rocket {rocketLaunchMessage.RocketModelId} with {rocketLaunchMessage.numberOfSlothsToLaunch} on board. Request id: {rocketLaunchMessage.messageId}. Retrying shortly.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error caught while processing message {JsonSerializer.Serialize(rocketLaunchMessage)}. Error: {ex.Message} ");
            }
        }
    }
}
