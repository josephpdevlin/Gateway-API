using Gateway.DB;
using Gateway.Domain;

namespace Gateway.Service
{
    public interface IRequestManager
    {
        Task<PaymentResponse> CreatePaymentRequest(PaymentRequest model);

        Task<PaymentResponse> GetPaymentRecord(int id);

        Task<PaymentResponse?> CheckForDuplicateRequest(string idempotencyKey);

        void CreateIdempotencyRecord(string idempotencyKey, Payment payment);

        Payment CreatePaymentRecordAndIdempotencyRecord(PaymentRequest payment);
    }
}