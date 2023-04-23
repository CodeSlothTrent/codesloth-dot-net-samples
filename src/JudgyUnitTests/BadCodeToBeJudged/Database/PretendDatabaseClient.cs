namespace BadCodeToBeJudged.Database
{
    // This class is a pretend database client, likely authored by an external party
    internal class PretendDatabaseClient
    {
        private string connectionString;
        private TimeSpan connectionTimeout;
        private TimeSpan queryTimeout;
        private int connectionRetries;
        private int queryRetries;

        public PretendDatabaseClient(string connectionString, TimeSpan connectionTimeout, TimeSpan queryTimeout, int connectionRetries, int queryRetries)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.connectionTimeout = connectionTimeout;
            this.queryTimeout = queryTimeout;
            this.connectionRetries = connectionRetries;
            this.queryRetries = queryRetries;
        }

        public async Task<DatabaseResult> FindDatabaseResult(string query)
        {
            // Pretend to take time making a db query
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            // Pretend that we find a single result. We don't care about this data for test purposes
            return new DatabaseResult(1);
        }

        public async Task<AnotherDatabaseResult> FindAnotherDatabaseResult(string query)
        {
            // Pretend to take time making a db query
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            // Pretend that we find a single result. We don't care about this data for test purposes
            return new AnotherDatabaseResult(1, 2, 3, 4, 5);
        }
    }
}
