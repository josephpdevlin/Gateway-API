using Gateway.Domain.Enums;

namespace Gateway.DB
{
    public class Payment
    {
        public int Id { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public PaymentStatus Status { get; set; } 

        public virtual Card Card { get; set; }
    }
}