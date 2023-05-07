using FixingConcreteDependencies.BusinessLogic;
using FixingConcreteDependencies.Database;
using FixingConcreteDependencies.Infrastructure;
using FixingConcreteDependencies.WebApi;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixingConcreteDependenciesUnitTests
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
            var rocketLaunchingLogic = new RocketLaunchingLogic(
                thrustCalculatorMock.Object, 
                rocketDatabaseRetrieverMock.Object,
                rocketQueuePollerMock.Object,
                rocketLaunchingServiceMock.Object,
                loggerMock.Object
            );

            // Oh no! This test will literally take 5 or more seconds to execute due to the async Task.Delay that it uses
        }
    }
}
