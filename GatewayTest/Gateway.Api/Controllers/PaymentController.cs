using Gateway.Api.Models;
using Gateway.Api.Services;
using Gateway.Api.Services.Validation;
using Gateway.DB;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IRequestManager _requestManager;
        private IRequestValidator _requestValidator;

        public PaymentController(IRequestManager requestManager, IRequestValidator requestValidator)
        {
            _requestManager = requestManager;
            _requestValidator = requestValidator;
        }

        [HttpGet]
        public async Task<ActionResult<Payment>> Get(int id)
        {
            return await _requestManager.GetPaymentRecord(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]string idempotencyKey, PaymentRequestModel model)
        {
            try
            {
                var validationErrors = _requestValidator.ValidateRequest(model);
                if(validationErrors.Count > 0)
                {
                    var errorModel = new PaymentRequestErrorModel()
                    {
                        Errors = validationErrors
                    };
                    return BadRequest(errorModel);
                }

                var response = await _requestManager.CreatePaymentRequest(model);
                return CreatedAtAction(nameof(Post), response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
