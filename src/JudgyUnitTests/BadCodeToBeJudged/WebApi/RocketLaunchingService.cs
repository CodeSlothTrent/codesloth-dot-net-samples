namespace BadCodeToBeJudged.WebApi
{
    internal class RocketLaunchingService
    {
        private HttpClient client;

        public RocketLaunchingService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<RocketLaunchResult> LaunchRocket(int rocketId, int numberOfSlothsToLaunch, int thrust, string food)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return new RocketLaunchResult(123, true);
        }
    }
}
