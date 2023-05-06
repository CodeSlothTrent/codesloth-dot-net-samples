using FixingTheInfiniteLoop.BusinessLogic;
using Microsoft.Extensions.Hosting;

namespace BadCodeToBeJudged
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
