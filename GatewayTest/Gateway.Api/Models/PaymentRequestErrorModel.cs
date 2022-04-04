namespace Gateway.Api.Models
{
    public class PaymentRequestErrorModel
    {
        public int Id { get; set; }
        public List<string> Errors { get; set; }
    }
}
