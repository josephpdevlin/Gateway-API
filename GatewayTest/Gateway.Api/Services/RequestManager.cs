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

        public async Task<BankResponseModel> CreatePaymentRequest(string idempotencyKey, PaymentRequestModel model)
        {
            var payment = _mapper.Map<Payment>(model);           
            payment.Status = PaymentStatus.Created;
            var dateTime = DateTime.UtcNow;
            payment.CreatedDateTime = dateTime;
            payment.LastUpdatedDateTime = dateTime;

            _repositoryManager.Insert(payment);

            var idempotencyRecord = new IdempotencyRecord()
            {
                IdempotencyKey = idempotencyKey,
                PaymentId = payment.Id,
                CreatedDateTime = dateTime,
            };

            _repositoryManager.InsertIdempotencyRecord(idempotencyRecord);

            var bankResponse = await SimulatedBank.ProcessPaymentRequest(model);
            bankResponse.PaymentId = payment.Id;
            _repositoryManager.AddBankResponse(bankResponse);
            _repositoryManager.UpdateStatus(payment.Id, bankResponse.Status);

            return _mapper.Map<BankResponseModel>(bankResponse);
        }

        public async Task<Payment> GetPaymentRecord(int id)
        {
            return await _repositoryManager.GetPayment(id);
        }

        public BankResponseModel CheckForDuplicateRequest(string idempotencyKey)
        {
            var idempotencyRecord = _repositoryManager.GetIdempotencyRecord(idempotencyKey);
            if (idempotencyRecord == null)
                return null;

            var response =  _repositoryManager.GetBankResponse(idempotencyRecord.PaymentId);

            return _mapper.Map<BankResponseModel>(response);
        }
    }
}
