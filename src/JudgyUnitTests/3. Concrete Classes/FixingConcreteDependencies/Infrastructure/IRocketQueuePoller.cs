namespace FixingConcreteDependencies.Infrastructure
{
    public interface IRocketQueuePoller
    {
        Task<RocketLaunchMessage> PollForRocketNeedingLaunch();
        Task RemoveMessageFromQueue(int messageId);
    }
}