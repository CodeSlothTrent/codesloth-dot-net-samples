using Microsoft.AspNetCore.Authentication;

namespace FixingStaticCode.BusinessLogic
{
    /// <summary>
    /// This simple class does not have any dependencies, it just takes inputs to perform a calculation
    /// Refactor candidate: consolidation with <seealso cref="CoordinateCalculator"/>
    /// </summary>
    public class ThrustCalculator : IThrustCalculator
    {
        private ISystemClock systemClock;

        public ThrustCalculator(ISystemClock systemClock)
        {
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        /// <summary>
        /// Calculate the thrust that is required to get the given number of sloths to the moon for the chosen rocket
        /// After lunch time, we need a little extra thrust to get through the night sky
        /// </summary>
        public int CalculateThrust(int input1, int input2, int input3, int input4, int input5, int numberOfSloths)
        {
            var result = (input1 + input2 + input3 + input4 + input5) * numberOfSloths;

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
}
