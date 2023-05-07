using FixingConcreteDependencies;
using FixingConcreteDependencies.BusinessLogic;
using FixingConcreteDependencies.Database;
using FixingConcreteDependencies.Infrastructure;
using FixingConcreteDependencies.WebApi;
using Microsoft.Extensions.Logging;

namespace FixingConcreteDependenciesUnitTests
{
    public class RocketLaunchingLogicTests
    {
        /// <summary>
        /// Now we can focus on writing tests for the rocket launching logic...or can we?
        /// </summary>
        [Fact]
        public async Task RocketLaunchingLogic_DoesSomeStuff()
        {
            var thrustCalculator = new ThrustCalculator();

            var pretendDatabaseClient = new PretendDatabaseClient(
                "connection string",
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(60),
                3,
                5
            );
            var loggerFactory = LoggerFactory.Create((loggingBuilder) => { });
            var rocketDatabaseRetrieverLogger = loggerFactory.CreateLogger<RocketDatabaseRetriever>();
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClient, rocketDatabaseRetrieverLogger);

            var rocketQueuePoller = new RocketQueuePoller(1, 2, "dependency info");

            var httpClient = new HttpClient();
            var rocketLaunchingService = new RocketLaunchingService(httpClient);

            var rocketLauncherLogger = loggerFactory.CreateLogger<RocketLauncher>();
            var rocketLaunchingLogic = new RocketLaunchingLogic(thrustCalculator, rocketDatabaseRetriever, rocketQueuePoller, rocketLaunchingService, rocketLauncherLogger);

            // Oh my... This much test setup does not even deserve a pretend call to our function!
            // Arrange (in Arrange, Act, Assert) should be concise. This much dependency construction is a code smell
        }
    }
}
