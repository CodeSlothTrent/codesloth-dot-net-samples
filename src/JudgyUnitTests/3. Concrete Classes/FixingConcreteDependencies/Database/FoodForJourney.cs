namespace BadCodeToBeJudged.Database
{
    public record FoodForJourney()
    {
        public int NumberOfCourses { get; init; }
        public string[] Foods { get; init; }
    };
}
