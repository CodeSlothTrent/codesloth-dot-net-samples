namespace BadCodeToBeJudged.BusinessLogic
{
    /// <summary>
    /// This simple class does not have any dependencies, it just takes inputs to perform a calculation
    /// Refactor candidate: consolidation with <seealso cref="StaticCoordinateCalculator"/>
    /// </summary>
    internal class ThrustCalculator
    {
        /// <summary>
        /// Calculate the thrust that is required to get the given number of sloths to the moon for the chosen rocket
        /// </summary>
        public int CalculateThrust(int input1, int input2, int input3, int input4, int input5, int numberOfSloths)
        {
            return (input1 + input2 + input3 + input4 + input5) * numberOfSloths;
        }
    }
}
