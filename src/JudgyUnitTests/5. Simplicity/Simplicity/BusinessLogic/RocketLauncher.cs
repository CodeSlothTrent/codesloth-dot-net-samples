using Simplicity.Database;
using Simplicity.Infrastructure;
using Simplicity.WebApi;

namespace Simplicity.BusinessLogic
{
    public class RocketLauncher : IRocketLauncher
    {
        private IThrustCalculator thrustCalculator;
        private IRocketDatabaseRetriever rocketDatabaseRetriever;
        private IRocketLaunchingService rocketLaunchingService;
        private ICoordinateCalculator coordinateCalculator;

        public RocketLauncher(IThrustCalculator thrustCalculator, IRocketDatabaseRetriever rocketDatabaseRetriever, IRocketLaunchingService rocketLaunchingService, ICoordinateCalculator coordinateCalculator)
        {
            this.thrustCalculator = thrustCalculator ?? throw new ArgumentNullException(nameof(thrustCalculator));
            this.rocketDatabaseRetriever = rocketDatabaseRetriever ?? throw new ArgumentNullException(nameof(rocketDatabaseRetriever));
            this.rocketLaunchingService = rocketLaunchingService ?? throw new ArgumentNullException(nameof(rocketLaunchingService));
            this.coordinateCalculator = coordinateCalculator ?? throw new ArgumentNullException(nameof(coordinateCalculator));
        }

        public async Task<RocketLaunchResult> LaunchARocket(RocketLaunchMessage rocketLaunchMessage)
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
            return rocketLaunchResult;
        }
    }
}
