using Gateway.Domain;

namespace Gateway.Service
{
    public interface IRequestManager
    {
        Task<PaymentResponse> CreatePaymentRequest(string idempotencyKey, PaymentRequest model);

        Task<PaymentResponse> GetPaymentRecord(int id);

        PaymentResponse CheckForDuplicateRequest(string idempotencyKey);
    }
}