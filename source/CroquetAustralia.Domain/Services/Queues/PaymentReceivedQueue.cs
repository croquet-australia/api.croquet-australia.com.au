namespace CroquetAustralia.Domain.Services.Queues
{
    public class PaymentReceivedQueue : QueueBase, IPaymentReceivedQueue
    {
        public const string QueueName = "payment-received";

        public PaymentReceivedQueue(IAzureStorageConnectionString connectionString)
            : base(QueueName, connectionString)
        {
        }
    }
}