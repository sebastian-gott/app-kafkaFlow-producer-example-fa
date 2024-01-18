
namespace KafkaFlowProducerHttpTrigger.Models
{
    public class Transaction
    {
        public string CustomerId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public double TransactionAmount { get; set; }
        public string Sender { get; set; }
    }
}
