using AutoMapper;
using Gateway.Api.Models;
using Gateway.DB;
using Gateway.Domain.Enums;

namespace Gateway.Api.Services
{
    public class RequestManager : IRequestManager
    {
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;

        public RequestManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<BankResponseModel> CreatePaymentRequest(PaymentRequestModel model)
        {
            var payment = _mapper.Map<Payment>(model);           
            payment.Status = PaymentStatus.Created;

            var dateTime = DateTime.UtcNow;
            payment.CreatedDateTime = dateTime;
            payment.LastUpdatedDateTime = dateTime;

            _repositoryManager.Insert(payment);

            var bankResponse = await SimulatedBank.ProcessPaymentRequest(model);
            _repositoryManager.UpdateStatus(payment.Id, bankResponse.Status);

            return bankResponse;
        }

        public async Task<Payment> GetPaymentRecord(int id)
        {
            return await _repositoryManager.Get(id);
        }
    }
}
