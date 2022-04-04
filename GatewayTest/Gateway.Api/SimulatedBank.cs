using Gateway.Api.Models;
using Gateway.Domain.Enums;

namespace Gateway.Api
{
    public static class SimulatedBank
    {
        public static async Task<BankResponseModel> ProcessPaymentRequest(PaymentRequestModel model)
        {
            if(model.Amount <= 100m)
            {
                return new BankResponseModel()
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
                return new BankResponseModel()
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
