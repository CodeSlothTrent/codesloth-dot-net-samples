namespace BadCodeToBeJudged.Infrastructure
{
    // This class pretends to fetch a message from a queue
    // To keep its contents simple, the types that it depends on are privitive types or complex types defined within this solution
    // In reality a class like this might depend on an API client wrapper publish via a NuGet package, such as AWS SQS's IAmazonSQS client
    // https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/ReceiveMessage.html
    internal class QueuePollingService
    {
        private int dependencyOne;
        private int dependencyTwo;
        private string dependencyThree;

        public QueuePollingService(int dependencyOne, int dependencyTwo, string dependencyThree)
        {
            this.dependencyOne = dependencyOne;
            this.dependencyTwo = dependencyTwo;
            this.dependencyThree = dependencyThree ?? throw new ArgumentNullException(nameof(dependencyThree));
        }

        /// <summary>
        /// Pretends to fetch a message from a queue. Returns null if a message is not found in expected time
        /// </summary>
        /// <returns></returns>
        public async Task<MessageFromQueue> GetMessageFromQueue()
        {
            // Pretend to long poll a queue for a message. This may take some time until a message becomes available
            await Task.Delay(TimeSpan.FromSeconds(5));
            // Return the fake message that we found
            return new MessageFromQueue(1, "Hi, I'm a fake message from a queue");
        }
    }
}
