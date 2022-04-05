using Gateway.Domain;
using Gateway.Service.Validation;
using NUnit.Framework;

namespace Gateway.Service.Test
{
    public class RequestValidatorTests
    {
        private RequestValidator validator;
        private PaymentRequest request;

        [SetUp]
        public void Setup()
        {
            request = new PaymentRequest()
            {
                Amount = 10,
                ExpiryMonth = 1,
                ExpiryYear = 2030,
                Currency = "GBP",
                CVV = 1234,
                Name = "J Devlin",
                Number = "4658584090000001"
            };

            validator = new RequestValidator();
        }

        [Test]
        public void ValidRequest_ReturnsNoErrors()
        {
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 0);
        }

        [Test]
        public void NegativeAmount_ReturnsError()
        {
            request.Amount = -10;
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 1);
            Assert.AreEqual(errorList[0], "Amount must be greater than 0");
        }

        [TestCase("GBP")]
        [TestCase("USD")]
        [TestCase("EUR")]
        public void SupportedCurrencyCode_ReturnsNoError(string currency)
        {
            request.Currency = currency;
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 0);
        }

        [Test]
        public void UnsupportedCurencyCode_ReturnsError()
        {
            request.Currency = "JPY";
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 1);
            Assert.AreEqual(errorList[0], "Unsupported currency");
        }

        [Test]
        public void NullName_ReturnsError()
        {
            request.Name = null;
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 1);
            Assert.AreEqual(errorList[0], "Name required");
        }

        [Test]
        public void ExpiredCard_ReturnsError()
        {
            request.ExpiryMonth = 3;
            request.ExpiryYear = 2022;
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 1);
            Assert.AreEqual(errorList[0], "Card expired");
        }

        [TestCase(12)]
        [TestCase(12345)]
        public void InvalidCvv_ReturnsError(int cvv)
        {
            request.CVV = cvv;
            var errorList = validator.ValidateRequest(request);
            Assert.That(errorList.Count == 1);
            Assert.AreEqual(errorList[0], "Invalid CVV");
        }
    }
}