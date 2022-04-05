using AutoMapper;
using Gateway.Api.Models;
using Gateway.Domain;
using Gateway.Service;
using Gateway.Service.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IRequestManager _requestManager;
        private IRequestValidator _requestValidator;
        private readonly IMapper _mapper;

        public PaymentController(IRequestManager requestManager, IRequestValidator requestValidator, IMapper mapper)
        {
            _requestManager = requestManager;
            _requestValidator = requestValidator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaymentResponseDto>> Get(int id)
        {
            try
            {
                var record = await _requestManager.GetPaymentRecord(id);
                return _mapper.Map<PaymentResponseDto>(record);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]string idempotencyKey, PaymentRequestDto model)
        {
            try
            {
                var duplicateRequest = await _requestManager.CheckForDuplicateRequest(idempotencyKey);
                if (duplicateRequest != null)
                {
                    return CreatedAtAction(nameof(Post), duplicateRequest);
                }

                var request = _mapper.Map<PaymentRequest>(model);

                var validationErrors = _requestValidator.ValidateRequest(request);
                if(validationErrors.Count > 0)
                {
                    var errorModel = new PaymentRequestErrorDto()
                    {
                        Errors = validationErrors
                    };
                    return BadRequest(errorModel);
                }

                var response = await _requestManager.CreatePaymentRequest(idempotencyKey, request);
                return CreatedAtAction(nameof(Post), response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
