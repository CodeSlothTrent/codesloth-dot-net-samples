using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Simplicity.BusinessLogic.Launch;
using Simplicity.Infrastructure;
using Simplicity.Infrastructure.DTO;
using Simplicity.WebApi.DTO;

namespace SimplicityUnitTests
{
    /// <summary>
    /// We are finally able to test the coordination of our rocket launches!
    /// </summary>
    public class RocketLaunchingCoordinatorTests
    {
        [Fact]
        public async Task RocketLaunchingCoordinator_DoesNotLaunchARocket_WhenNoMessageIsFound()
        {
            var rocketLauncherMock = new Mock<IRocketLauncher>();
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            var loggerMock = new Mock<ILogger<RocketLaunchingCoordinator>>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingCoordinator(
                rocketLauncherMock.Object,
                rocketQueuePollerMock.Object,
                loggerMock.Object,
                asyncDelayMock.Object
            );


            await rocketLaunchingLogic.TryToLaunchARocket();

            rocketQueuePollerMock.Verify(method => method.PollForRocketNeedingLaunch(), Times.Once, "We poll for a message once");
            asyncDelayMock.Verify(method => method.DelayAsync(It.IsAny<TimeSpan>()), Times.Once, "We should wait before returning early");

            // We most definitely do not want to try to launch a rocket if no trigger was found
            rocketLauncherMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RocketLaunchingCoordinator_PropagatesExceptions_WhenMessagePollingFails()
        {
            var rocketLauncherMock = new Mock<IRocketLauncher>();

            // Pretend that an exception is thrown
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            rocketQueuePollerMock.Setup(method => method.PollForRocketNeedingLaunch()).ThrowsAsync(new Exception("Pretend exception"));

            var loggerMock = new Mock<ILogger<RocketLaunchingCoordinator>>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingCoordinator(
                rocketLauncherMock.Object,
                rocketQueuePollerMock.Object,
                loggerMock.Object,
                asyncDelayMock.Object
            );

            // The logic of this test is not as clear as other tests, but is quite important.
            // If the infrastructure of the app fails, we should kill it, rather than swallowing the exception like we do during rocket launch failures
            // So that the app does not continue on an endless loop of failing to poll the queue
            Func<Task> act = () => rocketLaunchingLogic.TryToLaunchARocket();
            await act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RocketLaunchingCoordinator_LaunchesARocket_WhenMessageIsFound()
        {
            // Setup a fake message to be returned from our attempt to poll the queue
            var fakeRocketLaunchMessage = new RocketLaunchMessage(
                    messageId: 1,
                    numberOfSlothsToLaunch: 10,
                    RocketModelId: 2
                );
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            rocketQueuePollerMock.Setup(method => method.PollForRocketNeedingLaunch()).ReturnsAsync(fakeRocketLaunchMessage);

            // Configure the LaunchARocket mock method to capture the message that it was given
            RocketLaunchMessage? receivedMessage = null;
            var rocketLauncherMock = new Mock<IRocketLauncher>();
            rocketLauncherMock.Setup(method => method.LaunchARocket(It.IsAny<RocketLaunchMessage>()))
                .Callback<RocketLaunchMessage>(message => receivedMessage = message);

            var loggerMock = new Mock<ILogger<RocketLaunchingCoordinator>>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingCoordinator(
                rocketLauncherMock.Object,
                rocketQueuePollerMock.Object,
                loggerMock.Object,
                asyncDelayMock.Object
            );


            await rocketLaunchingLogic.TryToLaunchARocket();

            rocketLauncherMock.Verify(method => method.LaunchARocket(It.IsAny<RocketLaunchMessage>()), Times.Once(), "We launch the rocket that we found");
            // Confirm that we pass through the message that we received from the queue
            receivedMessage.Should().BeEquivalentTo(fakeRocketLaunchMessage);
        }

        [Fact]
        public async Task RocketLaunchingCoordinator_DoesNotRemoveLaunchMessageFromQueue_WhenRocketLaunchFails()
        {
            // Setup a fake message to be returned from our attempt to poll the queue
            var fakeRocketLaunchMessage = new RocketLaunchMessage(
                    messageId: 1,
                    numberOfSlothsToLaunch: 10,
                    RocketModelId: 2
                );
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            rocketQueuePollerMock.Setup(method => method.PollForRocketNeedingLaunch()).ReturnsAsync(fakeRocketLaunchMessage);

            // Pretend that our rocket launch was not successful
            var rocketLauncherMock = new Mock<IRocketLauncher>();
            rocketLauncherMock.Setup(method => method.LaunchARocket(It.IsAny<RocketLaunchMessage>()))
                .ReturnsAsync(new
                    RocketLaunchResult(
                        rocketId: 99,
                        launchWasSuccessful: false
                    )
            );

            var loggerMock = new Mock<ILogger<RocketLaunchingCoordinator>>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingCoordinator(
                rocketLauncherMock.Object,
                rocketQueuePollerMock.Object,
                loggerMock.Object,
                asyncDelayMock.Object
            );


            await rocketLaunchingLogic.TryToLaunchARocket();
            rocketQueuePollerMock.Verify(method => method.RemoveMessageFromQueue(It.IsAny<int>()), Times.Never, "We should not remove message from queue when launch fails");
        }

        [Fact]
        public async Task RocketLaunchingCoordinator_SwallowsExceptions_DuringRocketLaunchAttempt()
        {
            // Setup a fake message to be returned from our attempt to poll the queue
            var fakeRocketLaunchMessage = new RocketLaunchMessage(
                    messageId: 1,
                    numberOfSlothsToLaunch: 10,
                    RocketModelId: 2
                );
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            rocketQueuePollerMock.Setup(method => method.PollForRocketNeedingLaunch()).ReturnsAsync(fakeRocketLaunchMessage);

            // Pretend that an exception is thrown while launching the rocket
            var rocketLauncherMock = new Mock<IRocketLauncher>();
            rocketLauncherMock.Setup(method => method.LaunchARocket(It.IsAny<RocketLaunchMessage>()))
                .ThrowsAsync(new Exception("fake exception"));

            var loggerMock = new Mock<ILogger<RocketLaunchingCoordinator>>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingCoordinator(
                rocketLauncherMock.Object,
                rocketQueuePollerMock.Object,
                loggerMock.Object,
                asyncDelayMock.Object
            );

            // The swallowing of the exception is important, because we want to let the message reservation timeout lapse on the queue
            // and try to launch again at some point in the future. It should not be the responsibility of the app lifetime to know about this logic
            Func<Task> act = () => rocketLaunchingLogic.TryToLaunchARocket();
            await act.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task RocketLaunchingCoordinator_RemovesMessageFromQueue_WhenRocketLaunchSucceeds()
        {
            // Setup a fake message to be returned from our attempt to poll the queue
            var fakeRocketLaunchMessage = new RocketLaunchMessage(
                    messageId: 1,
                    numberOfSlothsToLaunch: 10,
                    RocketModelId: 2
                );
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            rocketQueuePollerMock.Setup(method => method.PollForRocketNeedingLaunch()).ReturnsAsync(fakeRocketLaunchMessage);

            // Pretend that our rocket launch was successful
            var rocketLauncherMock = new Mock<IRocketLauncher>();
            rocketLauncherMock.Setup(method => method.LaunchARocket(It.IsAny<RocketLaunchMessage>()))
                .ReturnsAsync(new
                    RocketLaunchResult(
                        rocketId: 99,
                        launchWasSuccessful: true
                    )
            );

            var loggerMock = new Mock<ILogger<RocketLaunchingCoordinator>>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingCoordinator(
                rocketLauncherMock.Object,
                rocketQueuePollerMock.Object,
                loggerMock.Object,
                asyncDelayMock.Object
            );


            await rocketLaunchingLogic.TryToLaunchARocket();
            rocketQueuePollerMock.Verify(method => method.RemoveMessageFromQueue(1), Times.Once, "We remove the message from the queue");
        }
    }
}