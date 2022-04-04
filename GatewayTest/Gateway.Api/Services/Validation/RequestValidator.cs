using Gateway.Api.Models;
using Gateway.DB;
using System.Text.RegularExpressions;

namespace Gateway.Api.Services.Validation
{
    public class RequestValidator : IRequestValidator
    {
        private IRepositoryManager _repositoryManager;

        public RequestValidator(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public List<string> ValidateRequest(PaymentRequestModel model)
        {
            var errorList = new List<string>();

            if (model.Amount < 0)
                errorList.Add("Amount must be greater than 0");

            var currency = model.Currency.ToUpper();
            if (currency != "GBP" && currency != "USD" && currency != "EUR")
                errorList.Add("Unsupported currency");

            if (string.IsNullOrEmpty(model.Card.Name))
                errorList.Add("Name required");

            var lastDayOfMonth = DateTime.DaysInMonth(model.Card.ExpiryYear, model.Card.ExpiryMonth);
            var cardExpiryDate = new DateTime(model.Card.ExpiryYear, model.Card.ExpiryMonth, lastDayOfMonth);
            if (cardExpiryDate < DateTime.UtcNow)
                errorList.Add("Card expired");

            var cvvRegex = "^[0-9]{3,4}$";
            var isValidCvv = Regex.Match(model.Card.CVV.ToString(), cvvRegex).Success;
            if (!isValidCvv)
                errorList.Add("Invalid CVV");

            return errorList;
        }

        public bool CheckIsRepeatRequest(string idempotencyKey)
        {
            return _repositoryManager.IsDuplicateRequest(idempotencyKey);
        }
    }
}
