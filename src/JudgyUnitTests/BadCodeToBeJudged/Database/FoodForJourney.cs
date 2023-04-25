namespace BadCodeToBeJudged.Database
{
    internal record FoodForJourney()
    {
        public int NumberOfCourses { get; init; }
        public string[] Foods { get; init; }
    };
}
