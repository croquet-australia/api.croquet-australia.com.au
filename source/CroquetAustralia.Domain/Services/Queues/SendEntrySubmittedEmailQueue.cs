namespace CroquetAustralia.Domain.Services.Queues
{
    public class SendEntrySubmittedEmailQueue : QueueBase, ISendEntrySubmittedEmailQueue
    {
        public const string QueueName = "send-entry-submitted-email";

        public SendEntrySubmittedEmailQueue(IAzureStorageConnectionString connectionString)
            : base(QueueName, connectionString)
        {
        }
    }
}