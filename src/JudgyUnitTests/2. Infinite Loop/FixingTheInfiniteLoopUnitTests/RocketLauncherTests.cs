using BadCodeToBeJudged;
using FixingTheInfiniteLoop.BusinessLogic;
using FluentAssertions;

namespace FixingTheInfiniteLoopUnitTests
{
    public class RocketLauncherTests
    {
        /// <summary>
        /// We've been able to write a test based on the loop! It's still not great, but we will fix this in coming tutorials
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RocketLauncher_StopsLaunchingRockets_WhenCancellationTokenIsSignaled()
        {
            var rocketLaunchingLogic = new RocketLaunchingLogic(null,null,null,null,null);
            var rocketLauncher = new RocketLauncher(rocketLaunchingLogic);
            
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            
            await rocketLauncher.StartAsync(cancellationToken);

            // A very silly assertion to confirm that we exited from StartAsync
            true.Should().BeTrue();
        }
    }
}
