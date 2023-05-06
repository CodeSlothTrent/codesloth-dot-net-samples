namespace BadCodeToBeJudged.Infrastructure
{
    public interface IRocketQueuePoller
    {
        Task<RocketLaunchMessage> PollForRocketNeedingLaunch();
        Task RemoveMessageFromQueue(int messageId);
    }
}