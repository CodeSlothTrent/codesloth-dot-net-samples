using FluentAssertions;

namespace FixingTheInfiniteLoopUnitTests
{
    public class MiniRocketLauncherTests
    {
        [Fact]
        public async Task RocketLauncher_IsHardToTest_AsBackgroundService()
        {
            var rocketLauncher = new MiniRocketLauncher();
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(1000);
            var token = tokenSource.Token;

            // Provide our token to the start method
            await rocketLauncher.StartAsync(token);

            rocketLauncher.LaunchCount.Should().Be(1);
            // Run 1: 167164137
            // Run 2: 161524603
            // Run 3: 169504777
        }
    }
}