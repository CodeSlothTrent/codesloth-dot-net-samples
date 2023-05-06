using BadCodeToBeJudged.Database;

namespace BadCodeToBeJudged.WebApi
{
    public interface IRocketLaunchingService
    {
        Task<RocketLaunchResult> LaunchRocket(int rocketId, int numberOfSlothsToLaunch, int thrust, FoodForJourney food);
    }
}