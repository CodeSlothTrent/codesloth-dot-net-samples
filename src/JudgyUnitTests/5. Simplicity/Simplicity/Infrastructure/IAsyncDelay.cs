namespace Simplicity.Infrastructure
{
    public interface IAsyncDelay
    {
        Task DelayAsync(TimeSpan duration);
    }
}
