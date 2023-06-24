using Microsoft.AspNetCore.Authentication;
using Simplicity.Database;

namespace Simplicity.BusinessLogic.Navigation
{
    /// <summary>
    /// A calcualtor of landing coordinates
    /// </summary>
    public class RocketNavigation : IRocketNavigation
    {
        private ISystemClock systemClock;
        private IRocketDatabaseRetriever rocketDatabaseRetriever;

        private record CoordinateInputs(int numberOfSloths, string foodForJourney);

        public RocketNavigation(ISystemClock systemClock, IRocketDatabaseRetriever rocketDatabaseRetriever)
        {
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            this.rocketDatabaseRetriever = rocketDatabaseRetriever ?? throw new ArgumentNullException(nameof(rocketDatabaseRetriever));
        }

        private (int latitude, int longitude) CalculateCoordinates(CoordinateInputs inputs) => inputs switch
        {
            { numberOfSloths: < 10, foodForJourney: PretendDatabaseClient.FoodSlightlyToxicLeaves } => (123, 456),
            { numberOfSloths: < 10, foodForJourney: PretendDatabaseClient.FoodSushi } => (456, 789),
            { numberOfSloths: > 10, foodForJourney: PretendDatabaseClient.FoodSushi } => (111, 222),
            { numberOfSloths: > 10, foodForJourney: PretendDatabaseClient.FoodSlightlyToxicLeaves } => (444, 444),
            { numberOfSloths: < 10 } => (000, 111),
            { numberOfSloths: >= 10 } => (111, 000)
        };

        /// <summary>
        /// We need to calculate the best place to land on the moon to accommodate the given number of sloths and their allocated cuisine
        /// </summary>
        public (int latitude, int longitude) CalculateCoordinatesToLand(int numberOfSloths, FoodForJourney foodForJourney)
        {
            if (foodForJourney == null)
            {
                throw new ArgumentNullException(nameof(foodForJourney));
            }

            var coordinates = new CoordinateInputs(numberOfSloths, foodForJourney.Foods.First());
            return CalculateCoordinates(coordinates);
        }

        public async Task<int> CalculateThrust(int rocketModelId, int numberOfSloths)
        {
            var statistics = await rocketDatabaseRetriever.FindRocketStatistics(rocketModelId);

            try
            {
                checked
                {
                    var result = (
                        statistics.ThrustValue1 +
                        statistics.ThrustValue2 +
                        statistics.ThrustValue3 +
                        statistics.ThrustValue4 +
                        statistics.ThrustValue5
                    ) * numberOfSloths;

                    // We have converted from a dependency on local time to a dependency on standardised UTC time
                    if (systemClock.UtcNow.Hour <= 2)
                    {
                        return result;
                    }
                    else
                    {
                        return result + 10;
                    }
                }
            }
            catch (OverflowException ex)
            {
                // This is redundant but demonstrates that we propagate the OverFlowException raised by the checked guard
                throw;
            }
        }
    }
}
