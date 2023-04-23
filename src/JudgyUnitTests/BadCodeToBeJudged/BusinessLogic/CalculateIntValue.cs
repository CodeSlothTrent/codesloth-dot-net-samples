namespace BadCodeToBeJudged.BusinessLogic
{
    /// <summary>
    /// This simple class does not have any dependencies, it just takes inputs to perform a calculation
    /// Refactor candidate: consolidation with <seealso cref="CalculateStringValue"/>
    /// </summary>
    internal class CalculateIntValue
    {
        /// <summary>
        /// Pretends to do a calculation on lots of inputs
        /// </summary>
        public int CalculateSomeNumber(int input1, int input2, int input3, int input4, int input5)
        {
            // Let's pretend that this method does a calculation on lots of inputs
            // To keep things simple though let's just return 1
            return 1;
        }
    }
}
