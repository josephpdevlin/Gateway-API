using AutoMapper;
using Gateway.DB;
using Gateway.Domain;
using Microsoft.Extensions.Logging;

namespace Gateway.Service
{
    public class RequestManager : IRequestManager
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<RequestManager> _logger;
        private readonly IMapper _mapper;

        public RequestManager(IRepositoryManager repositoryManager, ILogger<RequestManager> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentResponse> ProcessPaymentRequest(PaymentRequest paymentRequest)
        {
            var existingPaymentRecord = await CheckForDuplicateRequest(paymentRequest.IdempotencyKey);
            if (existingPaymentRecord != null)
                return existingPaymentRecord;

            var payment = CreatePaymentRecordAndIdempotencyRecord(paymentRequest);

            var bankResponse = await SimulatedBank.ProcessPaymentRequest(paymentRequest);
            if (bankResponse == null)
            {
                _logger.LogWarning("No response received from bank");
                return _mapper.Map<PaymentResponse>(payment);
            }

            _repositoryManager.UpdatePaymentRecord(payment.Id, bankResponse);

            var response = new PaymentResponse()
            {
                Id = payment.Id,
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
            recordModel.Id = record.Id;
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

        public Payment CreatePaymentRecordAndIdempotencyRecord(PaymentRequest paymentRequest)
        {
            var payment = _mapper.Map<Payment>(paymentRequest);

            payment.Status = "Created";
            var dateTime = DateTime.UtcNow;
            payment.CreatedDateTime = dateTime;
            payment.LastUpdatedDateTime = dateTime;

            payment.Number = MaskCardNumber(payment.Number);
            _repositoryManager.Insert(payment);

            CreateIdempotencyRecord(paymentRequest.IdempotencyKey, payment);
            return payment;
        }
        public void CreateIdempotencyRecord(string idempotencyKey, Payment payment)
        {
            var idempotencyRecord = new IdempotencyRecord()
            {
                PaymentId = payment.Id,
                IdempotencyKey = idempotencyKey,
                CreatedDateTime = DateTime.UtcNow
            };
            _repositoryManager.InsertIdempotencyRecord(idempotencyRecord);
        }

        private string MaskCardNumber(string number)
        {
            return "************" + number.Substring(12);
        }
    }
}
