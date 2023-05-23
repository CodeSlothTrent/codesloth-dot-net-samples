using FixingStaticCode.Database;

namespace FixingStaticCode.WebApi
{
    public interface IRocketLaunchingService
    {
        Task<RocketLaunchResult> LaunchRocket(int rocketId, int numberOfSlothsToLaunch, int thrust, FoodForJourney food);
    }
}