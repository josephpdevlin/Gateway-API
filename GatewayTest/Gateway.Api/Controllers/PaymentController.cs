using AutoMapper;
using Gateway.Api.Attributes;
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
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IRequestManager requestManager, IRequestValidator requestValidator, IMapper mapper, ILogger<PaymentController> logger)
        {
            _requestManager = requestManager;
            _requestValidator = requestValidator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiKey]
        public async Task<ActionResult<PaymentResponseDto>> Get(int id)
        {
            try
            {
                var record = await _requestManager.GetPaymentRecord(id);
                return _mapper.Map<PaymentResponseDto>(record);
            }
            catch(InvalidRequestException ex)
            {
                _logger.LogError("Error getting payment record: ", ex.Message);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error when attempting to retrieve payment record: ", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Triggers a payment request using details from the model
        /// </summary>
        /// <param name="idempotencyKey"></param>
        /// <param name="model"></param>
        /// <returns>201 with PaymentRequest model or 400 and an error message</returns>
        /// <remarks>
        /// Sample request:
        /// POST /PaymentRequestDto
        /// {
        ///     "amount": 100,
        ///     "currency": "gbp",
        ///     "name": "Joe Bloggs",
        ///     "number": "4658584090000001",
        ///     "expiryMonth": 12,
        ///     "expiryYear": 2023,
        ///     "cvv": 432
        /// }      
        ///
        /// </remarks>
        /// <response code = "201">Returns response for the newly created payment</response>
        /// <response code = "400">If the request model is missing required values or invalid card number</response>
        /// <response code = "418">If you ask the gateway to brew coffee</response>
        /// <response code = "422">If the request validation fails</response>
        /// <response code = "500">If a general server error occurs</response>
        [HttpPost]
        [ApiKey]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromHeader]string idempotencyKey, PaymentRequestDto model)
        {
            try
            {
                if (idempotencyKey == "brew coffee")
                    return StatusCode(StatusCodes.Status418ImATeapot);

                var request = _mapper.Map<PaymentRequest>(model);
                request.IdempotencyKey = idempotencyKey;

                var validationErrors = _requestValidator.ValidateRequest(request);
                if(validationErrors.Count > 0)
                {
                    var errorModel = new PaymentRequestErrorDto() { Errors = validationErrors };
                    return new JsonResult(errorModel) { StatusCode = StatusCodes.Status422UnprocessableEntity};
                }

                var response = await _requestManager.ProcessPaymentRequest(request);

                if (response.Status == "Succeeded" || response.Status == "Declined")
                    return new JsonResult(response) { StatusCode = StatusCodes.Status201Created };

                return new JsonResult(response) { StatusCode = StatusCodes.Status202Accepted};
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating payment request: ", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
