using AutoMapper;
using Gateway.Api.Mapper;
using Gateway.DB;
using Gateway.Domain;
using Gateway.Service.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Gateway.Service.Test
{
    public class RequestManagerTests
    {
        private string IdempotencyKey;
        private RequestManager manager;
        private PaymentRequest request;
        private IMapper mapper;
        private ILogger<RequestManager> logger;

        private Mock<IRepositoryManager> repositoryManagerMock;
        private Mock<IRequestManager> requestManagerMock;

        [SetUp]
        public void Setup()
        {
            IdempotencyKey = "123456test";

            request = new PaymentRequest()
            {
                IdempotencyKey=IdempotencyKey,
                Amount = 10,
                ExpiryMonth = 1,
                ExpiryYear = 2030,
                Currency = "GBP",
                CVV = 1234,
                Name = "J Devlin",
                Number = "4658584090000001"
            };
            
            var config = new MapperConfiguration(opts => { opts.AddProfile(new PaymentMapping()); });
            mapper = config.CreateMapper();
            repositoryManagerMock = new Mock<IRepositoryManager>();
            repositoryManagerMock.Setup(r => r.UpdateStatus(It.IsAny<int>(), It.IsAny<string>())).Verifiable();
            requestManagerMock = new Mock<IRequestManager>();
            requestManagerMock.Setup(r => r.CreatePaymentRecordAndIdempotencyRecord(It.IsAny<PaymentRequest>())).Returns(new Payment());
            requestManagerMock.Setup(r => r.CreateIdempotencyRecord(It.IsAny<string>(), It.IsAny<Payment>())).Verifiable();
            repositoryManagerMock.Setup(r => r.GetPayment(It.IsAny<int>())).Returns(Task.FromResult<Payment>(null));
            repositoryManagerMock.Setup(r => r.GetIdempotencyRecord(It.IsAny<string>())).Returns(new IdempotencyRecord());
            manager = new RequestManager(repositoryManagerMock.Object, logger, mapper);
        }

        [Test]
        public async Task ValidRequest_ReturnsNoErrorsAsync()
        {
            var actualResponse = await manager.CreatePaymentRequest(request);
            var expectedResponse = new PaymentResponse()
            {
                Amount = 10,
                ExpiryMonth = 1,
                ExpiryYear = 2030,
                IssuingBank = "Monzo Bank Ltd",
                Status = "Succeeded",
                Name = "J Devlin",
                Number = "4658584090000001",
                Id = 0
            };

            Assert.AreEqual(expectedResponse.Amount, actualResponse.Amount);
            Assert.AreEqual(expectedResponse.ExpiryMonth, actualResponse.ExpiryMonth);
            Assert.AreEqual(expectedResponse.ExpiryYear, actualResponse.ExpiryYear);
            Assert.AreEqual(expectedResponse.Name, actualResponse.Name);
            Assert.AreEqual(expectedResponse.Number, actualResponse.Number);
            Assert.AreEqual(expectedResponse.IssuingBank, actualResponse.IssuingBank);
            Assert.AreEqual(expectedResponse.Status, actualResponse.Status);
        }

        [Test]
        public void GetPaymentRecord_InvalidId_ReturnsException()
        {
            var exception = Assert.ThrowsAsync<InvalidRequestException>(async () => await manager.GetPaymentRecord(2));
            Assert.AreEqual("Invalid payment id", exception.Message);
        }

        [Test]
        public async Task CheckForDuplicateRequest_ValidIdempotencyKey_ReturnsPaymentResponse()
        {
            repositoryManagerMock.Setup(r => r.GetPayment(It.IsAny<int>())).Returns(Task.FromResult(new Payment()));
            var response = await manager.CheckForDuplicateRequest(IdempotencyKey);
            Assert.That(response, Is.TypeOf<PaymentResponse>());
        }

        [Test]
        public async Task CheckForDuplicateRequest_InvalidIdempotencyKey_ReturnsNull()
        {
            repositoryManagerMock.Setup(r => r.GetIdempotencyRecord(It.IsAny<string>())).Returns(() => null);
            var response = await manager.CheckForDuplicateRequest(IdempotencyKey);
            Assert.IsNull(response);
        }

        [Test]
        public void CreatePaymnetRecord_ValidPaymentRequest_ReturnsPaymentEntity()
        {
            repositoryManagerMock.Setup(r => r.Insert(It.IsAny<Payment>())).Verifiable();
            var payment = manager.CreatePaymentRecordAndIdempotencyRecord(request);
            Assert.AreEqual("Created", payment.Status);
            Assert.AreEqual(payment.CreatedDateTime, payment.LastUpdatedDateTime);
            Assert.AreEqual("************0001", payment.Number);
        }
    }
}