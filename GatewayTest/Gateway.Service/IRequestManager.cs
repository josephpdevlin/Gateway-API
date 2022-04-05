using Gateway.DB;
using Gateway.Domain;

namespace Gateway.Service
{
    public interface IRequestManager
    {
        Task<PaymentResponse> CreatePaymentRequest(string idempotencyKey, PaymentRequest model);

        Task<PaymentResponse> GetPaymentRecord(int id);

        Task<PaymentResponse?> CheckForDuplicateRequest(string idempotencyKey);

        void CreateIdempotencyRecord(string idempotencyKey, PaymentRequest payment);

        Payment CreatePaymentRecord(PaymentRequest payment);
    }
}