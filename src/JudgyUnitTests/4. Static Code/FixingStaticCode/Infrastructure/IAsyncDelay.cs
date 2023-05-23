namespace FixingStaticCode.Infrastructure
{
    public interface IAsyncDelay
    {
        Task DelayAsync(TimeSpan duration);
    }
}
