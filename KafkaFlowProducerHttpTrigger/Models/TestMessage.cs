using System.ComponentModel.DataAnnotations;

namespace KafkaFlowProducerHttpTrigger.Models
{
    public class TestMessage
    {
        [Required(ErrorMessage = "Message is missing")]
        public string Message { get; set; }

        [Required(ErrorMessage = "MessageNumber is missing")]
        public string MessageNumber { get; set; }
    }
}
