using Simplicity.Infrastructure.DTO;

namespace Simplicity.Infrastructure
{
    public interface IRocketQueuePoller
    {
        Task<RocketLaunchMessage> PollForRocketNeedingLaunch();
        Task RemoveMessageFromQueue(int messageId);
    }
}