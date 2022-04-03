namespace Gateway.Api.Models
{
    public class PaymentRequestModel
    {
        public int Id { get; set; }

        public int IndepotencyKey { get; set; }

        public int ApiKey { get; set; }

        public int CardNumber { get; set; }

        public string Expiry { get; set; }
    }
}
