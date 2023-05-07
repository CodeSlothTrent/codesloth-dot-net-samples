namespace FixingTheInfiniteLoop.Database
{
    // This class is a pretend database client, likely authored by an external party
    public class PretendDatabaseClient
    {
        private string connectionString;
        private TimeSpan connectionTimeout;
        private TimeSpan queryTimeout;
        private int connectionRetries;
        private int queryRetries;

        private readonly Random random = new Random();

        public PretendDatabaseClient(string connectionString, TimeSpan connectionTimeout, TimeSpan queryTimeout, int connectionRetries, int queryRetries)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.connectionTimeout = connectionTimeout;
            this.queryTimeout = queryTimeout;
            this.connectionRetries = connectionRetries;
            this.queryRetries = queryRetries;
        }

        private static string[] foods = new[] { "sushi", "pasta", "only slightly toxic leaves" };

        public async Task<FoodForJourney> FindFood(string query)
        {
            // Pretend to take time making a db query
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // Return a random result. This demonstrates that we don't have any control over our data source
            // and indicates that unit testing this component is not possible, as the output is not deterministic
            var numberOfCourses = random.Next(1, 3);
            var foodsToServe = new List<string>();
            for (int i = 0; i < numberOfCourses; i++)
            {
                var randomFoodIndex = random.NextInt64(0, foods.Length - 1);
                foodsToServe.Add(foods[randomFoodIndex]);
            }

            return new FoodForJourney { NumberOfCourses = numberOfCourses, Foods = foodsToServe.ToArray() };
        }

        public async Task<RocketThrustStatistics> FindRocketStatistics(string query)
        {
            // Pretend to take time making a db query
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // Return a random result. This demonstrates that we don't have any control over our data source
            // and indicates that unit testing this component is not possible, as the output is not deterministic
            return new RocketThrustStatistics(
                random.Next(),
                random.Next(),
                random.Next(),
                random.Next(),
                random.Next()
            );
        }
    }
}
