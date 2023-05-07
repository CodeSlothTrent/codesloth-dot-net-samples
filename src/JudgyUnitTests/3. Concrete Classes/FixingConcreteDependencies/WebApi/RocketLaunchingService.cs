using FixingConcreteDependencies.Database;

namespace FixingConcreteDependencies.WebApi
{
    public class RocketLaunchingService : IRocketLaunchingService
    {
        private HttpClient client;

        public RocketLaunchingService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<RocketLaunchResult> LaunchRocket(int rocketId, int numberOfSlothsToLaunch, int thrust, FoodForJourney food)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return new RocketLaunchResult(123, true);
        }
    }
}
