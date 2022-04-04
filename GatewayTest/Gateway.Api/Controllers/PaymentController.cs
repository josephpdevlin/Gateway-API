﻿using Gateway.Api.Models;
using Gateway.Api.Services;
using Gateway.Api.Services.Validation;
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
        public async Task<ActionResult<PaymentResponseModel>> Get(int id)
        {
            try
            {
                return await _requestManager.GetPaymentRecord(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]string idempotencyKey, PaymentRequestModel model)
        {
            try
            {
                var duplicateRequest = _requestManager.CheckForDuplicateRequest(idempotencyKey);
                if (duplicateRequest != null)
                {
                    return CreatedAtAction(nameof(Post), duplicateRequest);
                }

                var validationErrors = _requestValidator.ValidateRequest(model);
                if(validationErrors.Count > 0)
                {
                    var errorModel = new PaymentRequestErrorModel()
                    {
                        Errors = validationErrors
                    };
                    return BadRequest(errorModel);
                }

                var response = await _requestManager.CreatePaymentRequest(idempotencyKey, model);
                return CreatedAtAction(nameof(Post), response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
