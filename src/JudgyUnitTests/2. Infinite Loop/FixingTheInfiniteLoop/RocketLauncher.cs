using FixingTheInfiniteLoop.BusinessLogic;
using Microsoft.Extensions.Hosting;

namespace FixingTheInfiniteLoop
{
    public class RocketLauncher : BackgroundService
    {
        private RocketLaunchingLogic rocketLaunchingLogic;

        public RocketLauncher(RocketLaunchingLogic rocketLaunchingLogic)
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
