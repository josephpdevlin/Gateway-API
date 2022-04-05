# Gateway-API
A simple api that allows a merchant to submit payments for processing and retrieve details of previous payments.

This application is build using .NET 6 and makes use of an in memory database for the purpose of this task.

## Run
- Either open the solution in Visual Studio and run, or, navigate to ~/GatewayTest/Gateway.API and run the dotnet run command.
The application will start to run and Swagger can be accessed at https://localhost:5000.

- An api key is required to use the api. For the purpose of this task, the api key is set to 123. This can be set in the header 'api-key'.
A missing or invalid api key will result in a 401 Unauthorized.

- The main POST endpoint for processing a payment requires an idempotency key header 'idempotencyKey'. This ideally would be a UUID but can currently be any string value. This must be unique to each payment. If a payment has already been processed with a given idempotencyKey, the result of that payment will be returned and the submitted payment will not be processed. 

- The response from the POST to process a payment will provide you with a unique payment id. Use this with the GET endpoint to retrieve the details for that payment.

### Example Requests
#### POST
#### POST a successful payment request
```
Headers:
idempotencyKey = 1,
api-key = 123

Body:
{
  "amount": 10,
  "currency": "gbp",
  "name": "Joe Bloggs",
    "number": "4658584090000001",
    "expiryMonth": 12,
    "expiryYear": 2023,
    "cvv": 432
}
```
Submit the same request a second time and the response will be the same as the initial request, but the duplicate payment request will not have been processsed.
Change the idempotencyKey and the response Id will change, showing a new payment has been processed.

#### POST a declined payment request
The simulated bank will decline payments with an amount over 100.
```
Headers:
idempotencyKey = 10,
api-key = 123

Body:
{
  "amount": 101,
  "currency": "gbp",
  "name": "Joe Bloggs",
    "number": "4658584090000001",
    "expiryMonth": 12,
    "expiryYear": 2023,
    "cvv": 432
}
```

#### POST but the bank doesn't respond
The simulated bank will return a null response for card number 5555555555554444 to simulate a bank being unresponsive. The payment request will remain wil a status "Created" and a 202 response code as the payment has not yet been processed.

#### POST but the request did not include a required value
If a required value is missing such as card number, the api will return a 400 bad request withe an error detailing the reason for failing.

#### POST includes all the required data but something is not valid
If data in the payment request is invalid such as the card is expired or CVV is not valid, the api will return a 422 to state that the request was well-formed but contained invalid data.

All unhandled errors will return a 500 and log an exception.

#### GET
#### GET payment request by Id
- Use the Id returned in the response from the POST for a payment request. This Id is currently the primary key id for the entity, ideally this identitfier would be a guid or a reference coded that could be stored by a user.

## Assumptions Made
- Merchant will only process card payments, no other forms such as ApplePay, GooglePay etc.
- Payments will only be one time and not recurring.
- The gateway currently only handles GBP, USD and EUR.

## Improvements
- Time out for the idempotency key. The payment should be processed after a certain amount of time has passed such as 24 hours.
- Rate limiting for retries. If the same payment is retried a number of times, return 429 code to say too many requests.
- Further detail in the back simulator e.g. decline reasons
- API authorization handled and stored by a third party proviced such as Azure. This will allow users to make accounts.
- More detailed logging. This depends on what monitoring is used. When using Azure ApplicationInsights, the generated logs are often sufficient in place of manually created logs in some areas.
- Metrics will Azure Application Insights such as failure rates and response times.
- I included a yaml file for creating an Azure DevOps pipeline. 
- Build and run tests with DevOps, then deploy to an Azure AppService on the consumption plan so the service can scale vertically.
- Cache recent payments that are more likely to be accessed by a user.
- Integration tests for the full flow, possibly using SpecFlow with Given, When, Then Scenarios for main UseCases.
- More consideration to making use of custom exceptions that bubble up to the controller and dictate the resulting response.
- DI for DbContext.
- General refinement of function and property naming.
- More time on validation.
