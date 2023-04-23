namespace BadCodeToBeJudged.BusinessLogic
{
    /// <summary>
    /// This simple class does not have any dependencies, it just takes inputs to perform a calculation
    /// It has been defined as a static class, because someone thought it would be easier than having to inject it into another class through
    /// dependency injection
    /// Refactor candidate: consolidation with <seealso cref="CalculateIntValue"/>
    /// </summary>
    internal static class CalculateStringValue
    {
        /// <summary>
        /// Pretends to do some string calculation
        /// </summary>
        public static string MakeSomeAwesomeStringWithABigCalculation(string input1, string input2)
        {
            // Let's pretend that this function calculates some string value
            // To keep things simple return an empty string
            return "";
        }
    }
}
