namespace Gateway.Api.Models
{
    public class PaymentResponseModel
    {
        public int PaymentId { get; set; }

        public string Status { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string IssuingBank { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Number { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }
    }
}
