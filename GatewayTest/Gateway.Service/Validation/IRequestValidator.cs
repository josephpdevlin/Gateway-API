
using Gateway.Domain;

namespace Gateway.Service.Validation
{
    public interface IRequestValidator
    {
        List<string> ValidateRequest(PaymentRequest model);
    }
}