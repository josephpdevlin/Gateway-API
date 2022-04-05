namespace Gateway.Domain
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public int CVV { get; set; }
    }
}
