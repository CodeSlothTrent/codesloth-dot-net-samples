using Microsoft.Extensions.Logging;

namespace BadCodeToBeJudged.Database
{
    // This class pretends to fetch some data from a database
    // To keep its contents simple, the types that it depends on are privitive types or complex types defined within this solution
    // In reality a class like this might depend on an an externally published NuGet package, such as Dapper https://www.nuget.org/packages/Dapper/
    // or Entity Framework https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/
    internal class DatabaseService
    {
        private PretendDatabaseClient client;
        private ILogger<DatabaseService> logger;

        public DatabaseService(PretendDatabaseClient client, ILogger<DatabaseService> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// A method that queries a fake database client for some fake data. It is not critical, so returns null to express that there is no
        /// result so that the consumer can decide what to do
        /// </summary>
        public async Task<DatabaseResult?> GetNonCriticalDatabaseResult(int id)
        {
            try
            {
                return await client.FindDatabaseResult($"select * from table where id = {id}");
            }
            catch(Exception ex)
            {
                logger.LogError($"Exception caught while retrieving database result. {ex.Message}. Returning empty array.");
                return null;
            }
        }

        /// <summary>
        /// A method that queries a fake database client for some fake data. It is super critical, so we should let any exceptions bubble up
        /// and terminate the current flow
        /// </summary>
        public async Task<AnotherDatabaseResult> GetCriticalDatabaseResult(int id)
        {
            try
            {
                return await client.FindAnotherDatabaseResult($"select * from table where id = {id}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception caught while retrieving database result. {ex.Message}. Propagating exception as this is super bad oh no!");
                throw;
            }
        }
    }
}
