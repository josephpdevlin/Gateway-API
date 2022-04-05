using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Models
{
    public class PaymentRequestDto
    {
        [Required]
        [Range(0,int.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [CreditCard]
        public string Number { get; set; }

        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public int ExpiryYear { get; set; }

        [Required]
        public int CVV { get; set; }
    }
}
