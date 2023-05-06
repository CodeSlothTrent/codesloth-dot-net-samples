namespace BadCodeToBeJudged.Database
{
    public interface IRocketDatabaseRetriever
    {
        Task<RocketThrustStatistics> FindRocketStatistics(int rocketId);
        Task<FoodForJourney> GetFoodToFeedSlothsOnTheirJourney(int id, int numberOfSloths);
    }
}