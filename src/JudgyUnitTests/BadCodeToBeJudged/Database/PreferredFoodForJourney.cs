namespace BadCodeToBeJudged.Database
{
    internal record PreferredFoodForJourney()
    {
        public int NumberOfCourses { get; init; }
        public string[] Foods { get; init; }
    };
}
