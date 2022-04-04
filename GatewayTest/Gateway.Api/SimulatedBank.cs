using Gateway.Api.Models;
using Gateway.DB;
using Gateway.Domain.Enums;

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
                    Id = 0,
                    ProcessedDate = DateTime.Now,
                    Status = PaymentStatus.Succeded,
                    Amount = model.Amount,
                    IssuingBank = "Monzo Bank Ltd",
                    Name = model.Card.Name
                };
            }
            else
            {
                return new BankResponse()
                {
                    Id = 0,
                    ProcessedDate = DateTime.Now,
                    Status = PaymentStatus.Declined,
                    Amount = model.Amount,
                    IssuingBank = "Monzo Bank Ltd",
                    Name = model.Card.Name
                };
            }

        }
    }
}
