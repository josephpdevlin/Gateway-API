using Gateway.Domain.Enums;

namespace Gateway.DB
{
    public class BankResponse
    {
        public int Id { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string IssuingBank { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public int PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
