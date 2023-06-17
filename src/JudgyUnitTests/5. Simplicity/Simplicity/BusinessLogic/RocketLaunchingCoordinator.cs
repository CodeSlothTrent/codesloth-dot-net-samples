using Microsoft.Extensions.Logging;
using Simplicity.Infrastructure;
using System.Text.Json;

namespace Simplicity.BusinessLogic
{
    public class RocketLaunchingCoordinator : IRocketLaunchingCoordinator
    {
        private IRocketLauncher rocketLauncher;
        private IRocketQueuePoller rocketQueuePoller;
        private ILogger<RocketLaunchingCoordinator> logger;
        private IAsyncDelay asyncDelay;

        public RocketLaunchingCoordinator(IRocketLauncher rocketLauncher, IRocketQueuePoller rocketQueuePoller, ILogger<RocketLaunchingCoordinator> logger, IAsyncDelay asyncDelay)
        {
            this.rocketLauncher = rocketLauncher ?? throw new ArgumentNullException(nameof(rocketLauncher));
            this.rocketQueuePoller = rocketQueuePoller ?? throw new ArgumentNullException(nameof(rocketQueuePoller));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.asyncDelay = asyncDelay ?? throw new ArgumentNullException(nameof(asyncDelay));
        }

        public async Task TryToLaunchARocket()
        {
            var rocketLaunchMessage = await rocketQueuePoller.PollForRocketNeedingLaunch();
            if (rocketLaunchMessage == null)
            {
                await asyncDelay.DelayAsync(TimeSpan.FromSeconds(5));
                return;
            }

            try
            {
                var rocketLaunchResult = await rocketLauncher.LaunchARocket(rocketLaunchMessage);
                if (rocketLaunchResult.launchWasSuccessful)
                {
                    await rocketQueuePoller.RemoveMessageFromQueue(rocketLaunchMessage.messageId);
                }
                else
                {
                    throw new Exception($"Failed to launch rocket {rocketLaunchMessage.RocketModelId} with {rocketLaunchMessage.numberOfSlothsToLaunch} on board. Request id: {rocketLaunchMessage.messageId}. Retrying shortly.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error caught while processing message {JsonSerializer.Serialize(rocketLaunchMessage)}. Error: {ex.Message} ");
            }
        }
    }
}
