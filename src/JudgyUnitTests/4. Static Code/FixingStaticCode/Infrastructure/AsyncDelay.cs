namespace FixingStaticCode.Infrastructure
{
    public class AsyncDelay : IAsyncDelay
    {
        public async Task DelayAsync(TimeSpan duration)
        {
            await Task.Delay(duration);
        }
    }
}
