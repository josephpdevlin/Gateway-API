using Gateway.Api.Models;

namespace Gateway.Api.Services
{
    public interface IRequestManager
    {
        Task<PaymentResponseModel> CreatePaymentRequest(string idempotencyKey, PaymentRequestModel model);

        Task<PaymentResponseModel> GetPaymentRecord(int id);

        PaymentResponseModel CheckForDuplicateRequest(string idempotencyKey);
    }
}