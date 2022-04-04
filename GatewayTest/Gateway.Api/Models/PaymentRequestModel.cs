using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Models
{
    public class PaymentRequestModel
    {
        public string ReferenceKey { get; set; }

        public int IdempotencyKey { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public CardModel Card { get; set; }
    }
}
