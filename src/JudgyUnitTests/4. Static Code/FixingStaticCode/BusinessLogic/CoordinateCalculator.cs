using FixingStaticCode.Database;

namespace FixingStaticCode.BusinessLogic
{
    /// <summary>
    /// A calcualtor of landing coordinates
    /// </summary>
    public class CoordinateCalculator : ICoordinateCalculator
    {
        private record CoordinateInputs(int numberOfSloths, string foodForJourney);

        private (int latitude, int longitude) CalculateCoordinates(CoordinateInputs inputs) => inputs switch
        {
            { numberOfSloths: < 10, foodForJourney: "only slightly toxic leaves" } => (123, 456),
            { numberOfSloths: < 10, foodForJourney: "sushi" } => (456, 789),
            { numberOfSloths: > 10, foodForJourney: "sushi" } => (111, 222),
            { numberOfSloths: > 10, foodForJourney: "only slightly toxic leaves" } => (444, 444),
            { numberOfSloths: < 10 } => (000, 111),
            { numberOfSloths: > 10 } => (111, 000)
        };
        /// <summary>
        /// We need to calculate the best place to land on the moon to accommodate the given number of sloths and their allocated cuisine
        /// </summary>
        public (int latitude, int longitude) CalculateCoordinatesToLand(int numberOfSloths, FoodForJourney foodForJourney)
        {
            var coordinates = new CoordinateInputs(numberOfSloths, foodForJourney.Foods.First());
            return CalculateCoordinates(coordinates);
        }
    }
}
