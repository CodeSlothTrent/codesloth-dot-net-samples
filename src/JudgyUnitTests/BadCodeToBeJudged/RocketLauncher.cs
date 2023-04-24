using BadCodeToBeJudged.BusinessLogic;
using BadCodeToBeJudged.Database;
using BadCodeToBeJudged.Infrastructure;
using BadCodeToBeJudged.WebApi;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BadCodeToBeJudged
{
    /// <summary>
    /// The Code Sloth rocket launching station is a very busy department. 
    /// User's signal that they would like to launch one or more sloths into space by posting a message to a launch queue, 
    /// with information about the designated rocket to launch and how many sloths should be on board.
    /// 
    /// This class has grown in complexity over time, as the launch process has become more complex. A recent push for automated
    /// testing sees an engineer tasked with writing unit tests for the RocketLauncher.
    /// </summary>
    internal class RocketLauncher
    {
        private ThrustCalculator thrustCalculator;
        private RocketDatabaseRetriever rocketDatabaseRetriever;
        private RocketQueuePoller rocketQueuePoller;
        private RocketLaunchingService rocketLaunchingService;
        private ILogger<RocketLauncher> logger;

        public RocketLauncher(
            ThrustCalculator thrustCalculator, 
            RocketDatabaseRetriever rocketDatabaseRetriever, 
            RocketQueuePoller rocketQueuePoller, 
            RocketLaunchingService rocketLaunchingService, 
            ILogger<RocketLauncher> logger
        )
        {
            this.thrustCalculator = thrustCalculator ?? throw new ArgumentNullException(nameof(thrustCalculator));
            this.rocketDatabaseRetriever = rocketDatabaseRetriever ?? throw new ArgumentNullException(nameof(rocketDatabaseRetriever));
            this.rocketQueuePoller = rocketQueuePoller ?? throw new ArgumentNullException(nameof(rocketQueuePoller));
            this.rocketLaunchingService = rocketLaunchingService ?? throw new ArgumentNullException(nameof(rocketLaunchingService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async void LaunchRocketsToTheMoon(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var rocketLaunchMessage = await rocketQueuePoller.PollForRocketNeedingLaunch();
                if (rocketLaunchMessage == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    continue;
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

                    var preferredFoodForJourney = await rocketDatabaseRetriever.GetFoodToFeedSlothsOnTheirJourney(rocketLaunchMessage.RocketModelId, rocketLaunchMessage.numberOfSlothsToLaunch);
                    if (preferredFoodForJourney == null)
                    {
                        if (rocketLaunchMessage.numberOfSlothsToLaunch < 10)
                        {
                            preferredFoodForJourney = new PreferredFoodForJourney("only slightly toxic leaves");
                        }
                        else
                        {
                            throw new Exception("10 or more sloths require carefully planned food for space travel");
                        }
                    }

                    var coordinatesToLandOnMoon = StaticCoordinateCalculator.CalculateCoordinatesToLand(rocketLaunchMessage.numberOfSlothsToLaunch, preferredFoodForJourney.typeOfFoodToFeedSloths);

                    var rocketLaunchResult = await rocketLaunchingService.LaunchRocket(rocketLaunchMessage.RocketModelId, rocketLaunchMessage.numberOfSlothsToLaunch, requiredThrust, preferredFoodForJourney.typeOfFoodToFeedSloths);

                    if (rocketLaunchResult.launchWasSuccessful)
                    {
                        var result = await rocketQueuePoller.RemoveMessageFromQueue(rocketLaunchMessage.messageId);
                    } else
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
}
