using Gateway.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Gateway.DB
{
    public class Payment
    {
        public int Id { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        [MaxLength(16)]
        public string Number { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }
    }
}