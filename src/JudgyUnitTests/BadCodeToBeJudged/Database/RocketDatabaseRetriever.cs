using Microsoft.Extensions.Logging;

namespace BadCodeToBeJudged.Database
{
    // This class pretends to fetch some data from a database
    // In reality a class like this might depend on an an externally published NuGet package, such as Dapper https://www.nuget.org/packages/Dapper/
    // or Entity Framework https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/
    internal class RocketDatabaseRetriever
    {
        private PretendDatabaseClient client;
        private ILogger<RocketDatabaseRetriever> logger;

        public RocketDatabaseRetriever(PretendDatabaseClient client, ILogger<RocketDatabaseRetriever> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// A method that queries a fake database client to understand what food the sloths should be fed on their journey
        /// Because, you know, tailoring your menu to the venue and number of guests makes for a top notch journey to space
        /// Failing to retrieve this result is not catastrophic though, so don't throw an exception and instead let the user determine the 
        /// appropriate course of action
        /// </summary>
        public async Task<PreferredFoodForJourney?> GetFoodToFeedSlothsOnTheirJourney(int id, int numberOfSloths)
        {
            try
            {
                return await client.FindFood($"select * from foodTable where rocketId = {id} and numberOfSlothsToCaterFor = {numberOfSloths}");
            }
            catch(Exception ex)
            {
                logger.LogError($"Exception caught while determining food to feed sloths. {ex.Message}.");
                
                // Red flag
                return new PreferredFoodForJourney();
            }
        }

        /// <summary>
        /// A method that queries a fake database client for some fake data. It is super critical, so we should let any exceptions bubble up
        /// and terminate the current flow
        /// </summary>
        public async Task<RocketThrustStatistics> FindRocketStatistics(int rocketId)
        {
            try
            {
                return await client.FindRocketStatistics($"select * from table where id = {rocketId}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception caught while retrieving database result. {ex.Message}. Propagating exception as this is super bad oh no!");
                throw;
            }
        }
    }
}
