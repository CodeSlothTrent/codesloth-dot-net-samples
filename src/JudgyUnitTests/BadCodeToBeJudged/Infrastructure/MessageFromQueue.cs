namespace BadCodeToBeJudged.Infrastructure
{
    /// <summary>
    /// A fake message that is returned by the QueuePollingService
    /// </summary>
    internal record MessageFromQueue(int MessageId, string MessageContent);
}
