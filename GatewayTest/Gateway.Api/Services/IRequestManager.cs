using Gateway.Api.Models;
using Gateway.DB;

namespace Gateway.Api.Services
{
    public interface IRequestManager
    {
        Task<BankResponseModel> CreatePaymentRequest(PaymentRequestModel model);

        Task<Payment> GetPaymentRecord(int id);
    }
}