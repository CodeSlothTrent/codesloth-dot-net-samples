namespace Simplicity.WebApi
{
    public class RocketLaunchingApi : IRocketLaunchingApi
    {
        private HttpClient client;

        public RocketLaunchingApi(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<RocketLaunchResult> LaunchRocket(RocketLaunchRequest request)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return new RocketLaunchResult(123, true);
        }
    }
}
