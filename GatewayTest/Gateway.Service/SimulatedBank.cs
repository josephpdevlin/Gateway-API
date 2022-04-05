using Gateway.DB;
using Gateway.Domain;

namespace Gateway.Service
{
    public static class SimulatedBank
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<BankResponse?> ProcessPaymentRequest(PaymentRequest model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // suppressing warning here has with a real bank, the program would await call to bank

            if (model.Number == "5555555555554444")
            {
                return null;
            }
            else if (model.Amount <= 100m)
            {
                return new BankResponse()
                {
                    Status = "Succeeded",
                    Amount = model.Amount,
                    IssuingBank = "Monzo Bank Ltd"
                };
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
