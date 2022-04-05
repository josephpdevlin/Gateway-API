﻿using AutoMapper;
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

        public async Task<PaymentResponse> CreatePaymentRequest(string idempotencyKey, PaymentRequest model)
        {
            var payment = _mapper.Map<Payment>(model);
            CreatePaymentRecord(payment);

            var idempotencyRecord = new IdempotencyRecord()
            {
                IdempotencyKey = idempotencyKey,
                PaymentId = payment.Id,
                CreatedDateTime = payment.CreatedDateTime,
            };
            _repositoryManager.InsertIdempotencyRecord(idempotencyRecord);

            var bankResponse = await SimulatedBank.ProcessPaymentRequest(model);
            if(bankResponse == null)
                return _mapper.Map<PaymentResponse>(payment);

            _repositoryManager.UpdateStatus(payment.Id, bankResponse.Status);

            var response = new PaymentResponse()
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Name = payment.Name,
                Number = payment.Number,
                ExpiryMonth = payment.ExpiryMonth,
                ExpiryYear = payment.ExpiryYear,
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

        private void CreatePaymentRecord(Payment payment)
        {
            payment.Status = "Created";
            var dateTime = DateTime.UtcNow;
            payment.CreatedDateTime = dateTime;
            payment.LastUpdatedDateTime = dateTime;

            payment.Number = MaskCardNumber(payment.Number);
            _repositoryManager.Insert(payment);
        }

        public PaymentResponse CheckForDuplicateRequest(string idempotencyKey)
        {
            var idempotencyRecord = _repositoryManager.GetIdempotencyRecord(idempotencyKey);
            if (idempotencyRecord == null)
                return null;

            var response =  _repositoryManager.GetPayment(idempotencyRecord.PaymentId);

            return _mapper.Map<PaymentResponse>(response);
        }

        private string MaskCardNumber(string number)
        {
            return "************" + number.Substring(12);
        }
    }
}