using Gateway.Api.Models;
namespace Gateway.Api.Services.Validation
{
    public interface IRequestValidator
    {
        List<string> ValidateRequest(PaymentRequestModel model);
    }
}