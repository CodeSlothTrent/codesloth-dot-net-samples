namespace Simplicity.Database
{
    public interface IPretendDatabaseClient
    {
        Task<FoodForJourney> FindFood(string query);
        Task<RocketThrustStatistics> FindRocketStatistics(string query);
    }
}