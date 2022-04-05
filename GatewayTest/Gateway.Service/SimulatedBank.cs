using Gateway.DB;
using Gateway.Domain;

namespace Gateway.Service
{
    public static class SimulatedBank
    {
        public static async Task<BankResponse> ProcessPaymentRequest(PaymentRequest model)
        {
            if(model.Amount <= 100m)
            {
                return new BankResponse()
                {
                    Status = "Succeeded",
                    Amount = model.Amount,
                    IssuingBank = "Monzo Bank Ltd"
                };
            }
            else if (model.Number == "5555555555554444")
            {
                return null;
            }
            else
            {
                return new BankResponse()
                {
                    Status = "Declined",
                    Amount = model.Amount,
                    IssuingBank = "Monzo Bank Ltd"
                };
            }

        }
    }
}
