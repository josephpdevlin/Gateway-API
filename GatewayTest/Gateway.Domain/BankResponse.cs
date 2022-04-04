namespace Gateway.Domain
{
    public class BankResponse
    {
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string IssuingBank { get; set; }
        public DateTime ProcessedDateTime { get; set; }
    }
}
