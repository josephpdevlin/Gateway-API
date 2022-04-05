using AutoMapper;
using Gateway.DB;
using Gateway.Domain;

namespace Gateway.Service
{
    public class RequestManager : IRequestManager
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public RequestManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> CreatePaymentRequest(string idempotencyKey, PaymentRequest paymentRequest)
        {
            var payment = CreatePaymentRecord(paymentRequest);

            CreateIdempotencyRecord(idempotencyKey, paymentRequest);

            var bankResponse = await SimulatedBank.ProcessPaymentRequest(paymentRequest);
            if(bankResponse == null)
                return _mapper.Map<PaymentResponse>(paymentRequest);

            _repositoryManager.UpdateStatus(payment.Id, bankResponse.Status);

            var response = new PaymentResponse()
            {
                PaymentId = payment.Id,
                Amount = paymentRequest.Amount,
                Name = paymentRequest.Name,
                Number = paymentRequest.Number,
                ExpiryMonth = paymentRequest.ExpiryMonth,
                ExpiryYear = paymentRequest.ExpiryYear,
                ProcessedDate = payment.CreatedDateTime,
                Status = bankResponse.Status,
                IssuingBank = bankResponse.IssuingBank,
            };

            return response;
        }

        public async Task<PaymentResponse> GetPaymentRecord(int id)
        {

            var record = await _repositoryManager.GetPayment(id);
            if (record == null)
                throw new InvalidRequestException("Invalid payment id");

            var recordModel = _mapper.Map<PaymentResponse>(record);
            recordModel.PaymentId = record.Id;
            return recordModel;
        }

        public async Task<PaymentResponse?> CheckForDuplicateRequest(string idempotencyKey)
        {
            var idempotencyRecord = _repositoryManager.GetIdempotencyRecord(idempotencyKey);
            if (idempotencyRecord == null)
                return null;

            var response =  await _repositoryManager.GetPayment(idempotencyRecord.PaymentId);

            return _mapper.Map<PaymentResponse>(response);
        }

        public void CreateIdempotencyRecord(string idempotencyKey, PaymentRequest paymentRequest)
        {
            var idempotencyRecord = new IdempotencyRecord()
            {
                IdempotencyKey = idempotencyKey,
                CreatedDateTime = DateTime.UtcNow
            };
            _repositoryManager.InsertIdempotencyRecord(idempotencyRecord);
        }

        public Payment CreatePaymentRecord(PaymentRequest paymentRequest)
        {
            var payment = _mapper.Map<Payment>(paymentRequest);

            payment.Status = "Created";
            var dateTime = DateTime.UtcNow;
            payment.CreatedDateTime = dateTime;
            payment.LastUpdatedDateTime = dateTime;

            payment.Number = MaskCardNumber(payment.Number);
            _repositoryManager.Insert(payment);
            return payment;
        }

        private string MaskCardNumber(string number)
        {
            return "************" + number.Substring(12);
        }
    }
}
