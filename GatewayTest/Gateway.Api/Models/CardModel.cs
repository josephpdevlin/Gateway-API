using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Models
{
    public class CardModel
    {
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
