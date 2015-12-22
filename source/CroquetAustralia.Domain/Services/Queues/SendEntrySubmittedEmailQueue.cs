namespace CroquetAustralia.Domain.Services.Queues
{
    public class SendEntrySubmittedEmailQueue : QueueBase, ISendEntrySubmittedEmailQueue
    {
        public SendEntrySubmittedEmailQueue(IAzureStorageConnectionString connectionString)
            : base("send-entry-submitted-email", connectionString)
        {
        }
    }
}