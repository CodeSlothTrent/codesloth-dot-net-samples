using FixingStaticCode.BusinessLogic;
using FixingStaticCode.Database;
using FixingStaticCode.Infrastructure;
using FixingStaticCode.WebApi;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixingStaticCodeUnitTests
{
    /// <summary>
    /// Test setup has become much simpler for our RocketLaunchingLogic class now!
    /// </summary>
    public class RocketLaunchingLogicTests
    {
        [Fact]
        public async Task RocketLaunchingLogic_TriesToLaunchARocketAgain_IfItDoesNotInitiallyFindOne()
        {
            var thrustCalculatorMock = new Mock<IThrustCalculator>();
            var rocketDatabaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            var rocketLaunchingServiceMock = new Mock<IRocketLaunchingService>();
            var loggerMock = new Mock<ILogger<RocketLaunchingLogic>>();
            var coordinateCalculatorMock = new Mock<ICoordinateCalculator>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingLogic(
                thrustCalculatorMock.Object,
                rocketDatabaseRetrieverMock.Object,
                rocketQueuePollerMock.Object,
                rocketLaunchingServiceMock.Object,
                loggerMock.Object,
                coordinateCalculatorMock.Object,
                asyncDelayMock.Object
            );

            // Oh no! This test will literally take 5 or more seconds to execute due to the async Task.Delay that it uses
        }
    }
}
