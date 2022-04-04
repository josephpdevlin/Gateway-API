using Gateway.Api.Models;
using Gateway.Domain;

namespace Gateway.Api
{
    public static class SimulatedBank
    {
        public static async Task<BankResponse> ProcessPaymentRequest(PaymentRequestModel model)
        {
            if(model.Amount <= 100m)
            {
                return new BankResponse()
                {
                    Status = "Succeded",
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
