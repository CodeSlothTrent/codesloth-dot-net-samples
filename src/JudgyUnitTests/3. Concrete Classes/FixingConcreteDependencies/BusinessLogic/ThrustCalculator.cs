namespace FixingConcreteDependencies.BusinessLogic
{
    /// <summary>
    /// This simple class does not have any dependencies, it just takes inputs to perform a calculation
    /// Refactor candidate: consolidation with <seealso cref="StaticCoordinateCalculator"/>
    /// </summary>
    public class ThrustCalculator : IThrustCalculator
    {
        /// <summary>
        /// Calculate the thrust that is required to get the given number of sloths to the moon for the chosen rocket
        /// After lunch time, we need a little extra thrust to get through the night sky
        /// </summary>
        public int CalculateThrust(int input1, int input2, int input3, int input4, int input5, int numberOfSloths)
        {
            var result = (input1 + input2 + input3 + input4 + input5) * numberOfSloths;
            if (DateTime.Now.Hour <= 12)
            {
                return result;
            } else
            {
                return result + 10;
            }
        }
    }
}
