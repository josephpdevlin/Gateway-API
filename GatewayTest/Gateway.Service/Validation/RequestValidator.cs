﻿using Gateway.Domain;
using System.Text.RegularExpressions;

namespace Gateway.Service.Validation
{
    public class RequestValidator : IRequestValidator
    {
        public List<string> ValidateRequest(PaymentRequest model)
        {
            var errorList = new List<string>();

            if (model.Amount < 0)
                errorList.Add("Amount must be greater than 0");

            var currency = model.Currency.ToUpper();
            if (currency != "GBP" && currency != "USD" && currency != "EUR")
                errorList.Add("Unsupported currency");

            if (string.IsNullOrEmpty(model.Name))
                errorList.Add("Name required");

            var lastDayOfMonth = DateTime.DaysInMonth(model.ExpiryYear, model.ExpiryMonth);
            var cardExpiryDate = new DateTime(model.ExpiryYear, model.ExpiryMonth, lastDayOfMonth);
            if (cardExpiryDate < DateTime.UtcNow)
                errorList.Add("Card expired");

            var cvvRegex = "^[0-9]{3,4}$";
            var isValidCvv = Regex.Match(model.CVV.ToString(), cvvRegex).Success;
            if (!isValidCvv)
                errorList.Add("Invalid CVV");

            return errorList;
        }
    }
}
