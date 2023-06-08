using FluentAssertions;
using Moq;
using Simplicity;
using Simplicity.BusinessLogic;

namespace SimplicityUnitTests
{
    /// <summary>
    /// Now that we depend on an interface we have the ability to write more unit tests
    /// This is because we can simulate different flows through the StartAsync method with a Mock and fake data
    /// </summary>
    public class RocketLauncherTests
    {
        /// <summary>
        /// The same test from before, except this time we have an objective assertion
        /// </summary>
        [Fact]
        public async Task RocketLauncher_DoesNotLaunchARocket_WhenGivenACancelledToken()
        {
            var rocketLaunchingLogicMock = new Mock<IRocketLaunchingLogic>();
            var rocketLauncher = new RocketLauncher(rocketLaunchingLogicMock.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();

            await rocketLauncher.StartAsync(cancellationToken);

            // We should never launch a rocket if our tokenis already cancelled
            rocketLaunchingLogicMock.Verify(method => method.TryToLaunchARocket(), Times.Never());
        }

        /// <summary>
        /// We can now objectively assert that we terminate the loop after the token is singalled
        /// </summary>
        [Fact]
        public async Task RocketLauncher_StopsLaunchingRockets_WhenCancellationTokenIsSignaled()
        {
            var rocketLaunchingLogicMock = new Mock<IRocketLaunchingLogic>();
            var rocketLauncher = new RocketLauncher(rocketLaunchingLogicMock.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.CancelAfter(1000);

            await rocketLauncher.StartAsync(cancellationToken);

            // We still lack determinism through the usage of cancellation token, but can assert against something more concrete
            rocketLaunchingLogicMock.Verify(method => method.TryToLaunchARocket(), Times.AtLeastOnce());
        }

        /// <summary>
        /// One testing flow that is often overlooked is: what happens when an exception is thrown?
        /// </summary>
        [Fact]
        public async Task RocketLauncher_PropagatesUnhandledException_WhenTheyAreThrownByRocketLaunchingLogic()
        {
            var rocketLaunchingLogicMock = new Mock<IRocketLaunchingLogic>();

            // When the rocket launching logic interface is invoked, our mock will throw an exception
            rocketLaunchingLogicMock
                .Setup(method => method.TryToLaunchARocket())
                .ThrowsAsync(new Exception("A fake exception to be thrown when the mock is called"));

            var rocketLauncher = new RocketLauncher(rocketLaunchingLogicMock.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            // We do not need to specify cancellation conditions, as the exception is expected to break us from the loop
            var cancellationToken = cancellationTokenSource.Token;

            Func<Task> act = () => rocketLauncher.StartAsync(cancellationToken);

            // Fluent assertions catches the exception which bubbles out of the RocketLauncher
            await act.Should().ThrowAsync<Exception>();

            // We can assert with determinism that this flow only calls TryToLaunchRocket once
            rocketLaunchingLogicMock.Verify(method => method.TryToLaunchARocket(), Times.Once());
        }
    }
}
