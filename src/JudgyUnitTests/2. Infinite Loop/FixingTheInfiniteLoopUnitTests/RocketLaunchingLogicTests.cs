using FixingTheInfiniteLoop;
using FixingTheInfiniteLoop.BusinessLogic;
using FixingTheInfiniteLoop.Database;
using FixingTheInfiniteLoop.Infrastructure;
using FixingTheInfiniteLoop.WebApi;
using Microsoft.Extensions.Logging;

namespace FixingTheInfiniteLoopUnitTests
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
            var rocketDatabaseRetrieverLogger = LoggerFactoryExtensions.CreateLogger<RocketDatabaseRetriever>(loggerFactory);
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClient, rocketDatabaseRetrieverLogger);

            var rocketQueuePoller = new RocketQueuePoller(1, 2, "dependency info");

            var httpClient = new HttpClient();
            var rocketLaunchingService = new RocketLaunchingService(httpClient);

            var rocketLauncherLogger = LoggerFactoryExtensions.CreateLogger<RocketLauncher>(loggerFactory);
            var rocketLaunchingLogic = new RocketLaunchingLogic(thrustCalculator, rocketDatabaseRetriever, rocketQueuePoller, rocketLaunchingService, rocketLauncherLogger);

            // Oh my... This much test setup does not even deserve a pretend call to our function!
            // Arrange (in Arrange, Act, Assert) should be concise. This much dependency construction is a code smell
        }

        /// <summary>
        /// An example of a setup method that may be engineered to comabt the complex test data setup. This is a code smell and sould be avoided!
        /// </summary>
        private RocketLaunchingLogic MakeRocketLaunchingLogicInitialImplementation()
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
            var rocketDatabaseRetrieverLogger = LoggerFactoryExtensions.CreateLogger<RocketDatabaseRetriever>(loggerFactory);
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClient, rocketDatabaseRetrieverLogger);

            var rocketQueuePoller = new RocketQueuePoller(1, 2, "dependency info");

            var httpClient = new HttpClient();
            var rocketLaunchingService = new RocketLaunchingService(httpClient);

            var rocketLauncherLogger = LoggerFactoryExtensions.CreateLogger<RocketLauncher>(loggerFactory);
            var rocketLaunchingLogic = new RocketLaunchingLogic(thrustCalculator, rocketDatabaseRetriever, rocketQueuePoller, rocketLaunchingService, rocketLauncherLogger);
            return rocketLaunchingLogic;
        }

        /// <summary>
        /// As the test suite grows over time, this helper method will likely become parameterised. Perhaps it may make sense at first.
        /// </summary>
        private RocketLaunchingLogic MakeRocketLaunchingLogicSecondIteration(string connectionString, int durationSeconds)
        {
            var thrustCalculator = new ThrustCalculator();

            var pretendDatabaseClient = new PretendDatabaseClient(
                connectionString,
                TimeSpan.FromSeconds(durationSeconds),
                TimeSpan.FromSeconds(60),
                3,
                5
            );
            var loggerFactory = LoggerFactory.Create((loggingBuilder) => { });
            var rocketDatabaseRetrieverLogger = LoggerFactoryExtensions.CreateLogger<RocketDatabaseRetriever>(loggerFactory);
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClient, rocketDatabaseRetrieverLogger);

            var rocketQueuePoller = new RocketQueuePoller(1, 2, "dependency info");

            var httpClient = new HttpClient();
            var rocketLaunchingService = new RocketLaunchingService(httpClient);

            var rocketLauncherLogger = LoggerFactoryExtensions.CreateLogger<RocketLauncher>(loggerFactory);
            var rocketLaunchingLogic = new RocketLaunchingLogic(thrustCalculator, rocketDatabaseRetriever, rocketQueuePoller, rocketLaunchingService, rocketLauncherLogger);
            return rocketLaunchingLogic;
        }

        /// <summary>
        /// There we have it, the nullable parameters begin, logic creeps into this method and before we know it nobdy knows what this data means, what this method does, or how it impacts a given test case
        /// </summary>
        private RocketLaunchingLogic MakeRocketLaunchingLogicThirdIteration(string connectionString, int durationSeconds, string? dependencyInfo)
        {
            var thrustCalculator = new ThrustCalculator();

            var pretendDatabaseClient = new PretendDatabaseClient(
                connectionString,
                TimeSpan.FromSeconds(durationSeconds),
                TimeSpan.FromSeconds(60),
                3,
                5
            );
            var loggerFactory = LoggerFactory.Create((loggingBuilder) => { });
            var rocketDatabaseRetrieverLogger = LoggerFactoryExtensions.CreateLogger<RocketDatabaseRetriever>(loggerFactory);
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClient, rocketDatabaseRetrieverLogger);

            var rocketQueuePoller = new RocketQueuePoller(1, 2, dependencyInfo ?? "dependency info");

            var httpClient = new HttpClient();
            var rocketLaunchingService = new RocketLaunchingService(httpClient);

            var rocketLauncherLogger = LoggerFactoryExtensions.CreateLogger<RocketLauncher>(loggerFactory);
            var rocketLaunchingLogic = new RocketLaunchingLogic(thrustCalculator, rocketDatabaseRetriever, rocketQueuePoller, rocketLaunchingService, rocketLauncherLogger);
            return rocketLaunchingLogic;
        }
    }
}
