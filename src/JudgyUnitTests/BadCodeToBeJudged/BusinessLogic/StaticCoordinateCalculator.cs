namespace BadCodeToBeJudged.BusinessLogic
{
    /// <summary>
    /// This simple class does not have any dependencies, it just takes inputs to perform a calculation
    /// It has been defined as a static class, because someone thought it would be easier than having to inject it into another class through
    /// dependency injection
    /// Refactor candidate: consolidation with <seealso cref="ThrustCalculator"/>
    /// </summary>
    internal static class StaticCoordinateCalculator
    {
        /// <summary>
        /// We need to calculate the best place to land on the moon to accommodate the given number of sloths and their allocated cuisine
        /// </summary>
        public static (int latitude, int longitude) CalculateCoordinatesToLand(int numberOfSloths, string foodForJourney)
        {
            // Let's pretend that this function calculates some string value
            // To keep things simple return an empty string
            return (123,876);
        }
    }
}
