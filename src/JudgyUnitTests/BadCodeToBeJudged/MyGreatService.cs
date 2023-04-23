using BadCodeToBeJudged.BusinessLogic;
using BadCodeToBeJudged.Database;
using BadCodeToBeJudged.Infrastructure;
using BadCodeToBeJudged.WebApi;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BadCodeToBeJudged
{
    /// <summary>
    /// A class that has been extended to its limit over time
    /// </summary>
    internal class MyGreatService
    {
        private CalculateIntValue calculateIntValue;
        private DatabaseService databaseService;
        private QueuePollingService queuePollingService;
        private WebApiService webApiService;
        private AnotherWebApiService anotherWebApiService;
        private ILogger<MyGreatService> logger;

        public MyGreatService(
            CalculateIntValue calculateIntValue,
            DatabaseService databaseService,
            QueuePollingService queuePollingService,
            WebApiService webApiService,
            AnotherWebApiService anotherWebApiService,
            ILogger<MyGreatService> logger
        )
        {
            this.calculateIntValue = calculateIntValue ?? throw new ArgumentNullException(nameof(calculateIntValue));
            this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            this.queuePollingService = queuePollingService ?? throw new ArgumentNullException(nameof(queuePollingService));
            this.webApiService = webApiService ?? throw new ArgumentNullException(nameof(webApiService));
            this.anotherWebApiService = anotherWebApiService ?? throw new ArgumentNullException(nameof(anotherWebApiService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async void DoSomeGreatThings(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var queueResult = await queuePollingService.GetMessageFromQueue();
                if (queueResult == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    continue;
                }

                try
                {
                    var databaseResult = await databaseService.GetCriticalDatabaseResult(queueResult.MessageId);
                    var intResult = calculateIntValue.CalculateSomeNumber(
                        databaseResult.Value1,
                        databaseResult.Value2,
                        databaseResult.Value3,
                        databaseResult.Value4,
                        databaseResult.Value5
                    );

                    // If we fail to get a non critical result, then set its value to 1 so our app can keep moving forward
                    var nonCriticalDatabaseResult = await databaseService.GetNonCriticalDatabaseResult(queueResult.MessageId)
                        ?? new DatabaseResult(1);

                    var stringResult = CalculateStringValue.MakeSomeAwesomeStringWithABigCalculation(
                        databaseResult.Value1.ToString(),
                        nonCriticalDatabaseResult.ToString()
                    );

                    // Todo: add web api calls
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error caught while processing message {JsonSerializer.Serialize(queueResult)}. Error: {ex.Message} ");
                }
            }

        }
    }
}
