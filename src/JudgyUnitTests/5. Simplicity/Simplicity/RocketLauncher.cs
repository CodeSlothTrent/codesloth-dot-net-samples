using Microsoft.Extensions.Hosting;
using Simplicity.BusinessLogic;

namespace Simplicity
{
    public class RocketLauncher : BackgroundService
    {
        private IRocketLaunchingLogic rocketLaunchingLogic;

        public RocketLauncher(IRocketLaunchingLogic rocketLaunchingLogic)
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
