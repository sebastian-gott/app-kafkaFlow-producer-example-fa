using System.ComponentModel.DataAnnotations;

namespace KafkaFlowProducerHttpTrigger.Models
{
    public class Order
    {
        [Required(ErrorMessage = "CustomerId is missing")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Order CreatedUtc is missing")]
        public DateTime CreatedUtc { get; set; }

        [Required(ErrorMessage = "ProductNumber is missing")]
        public string ProductNumber { get; set; }

        [Required(ErrorMessage = "OrderQuantity is missing")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderQuantity must be greater than 0")]
        public int OrderQuantity { get; set; }
    }
}
