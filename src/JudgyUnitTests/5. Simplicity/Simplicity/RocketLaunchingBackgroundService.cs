using Microsoft.Extensions.Hosting;
using Simplicity.BusinessLogic.Launch;

namespace Simplicity
{
    public class RocketLaunchingBackgroundService : BackgroundService
    {
        private IRocketLaunchingCoordinator rocketLaunchingLogic;

        public RocketLaunchingBackgroundService(IRocketLaunchingCoordinator rocketLaunchingLogic)
        {
            this.rocketLaunchingLogic = rocketLaunchingLogic ?? throw new ArgumentNullException(nameof(rocketLaunchingLogic));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await rocketLaunchingLogic.TryToLaunchARocket();
            }
        }
    }
}
