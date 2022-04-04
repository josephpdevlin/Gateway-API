namespace Gateway.Api.Models
{
    public class PaymentRequestErrorDto
    {
        public int Id { get; set; }
        public List<string> Errors { get; set; }
    }
}
