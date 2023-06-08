using Simplicity.Database;

namespace Simplicity.WebApi
{
    public interface IRocketLaunchingService
    {
        Task<RocketLaunchResult> LaunchRocket(int rocketId, int numberOfSlothsToLaunch, int thrust, FoodForJourney food);
    }
}