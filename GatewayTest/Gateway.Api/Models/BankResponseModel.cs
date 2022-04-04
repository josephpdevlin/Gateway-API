using Gateway.Domain.Enums;

namespace Gateway.Api.Models
{
    public class BankResponseModel
    {
        public int Id { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string IssuingBank { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}
