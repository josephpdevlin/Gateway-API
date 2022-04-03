using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        [HttpGet]
        public async Task<HttpResponseMessage> Post()
        {

            return new HttpResponseMessage();
        }
    }
}
