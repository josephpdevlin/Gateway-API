using AutoMapper;
using Gateway.Api.Models;
using Gateway.DB;

namespace Gateway.Api.Mapper
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<PaymentRequestDto, Payment>();
            CreateMap<PaymentResponseDto, Payment>().ReverseMap();
        }
    }
}
