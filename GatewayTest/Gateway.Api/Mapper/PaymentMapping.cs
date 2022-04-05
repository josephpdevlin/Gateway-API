using AutoMapper;
using Gateway.Api.Models;
using Gateway.DB;
using Gateway.Domain;

namespace Gateway.Api.Mapper
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<PaymentRequest, PaymentRequestDto>().ReverseMap();
            CreateMap<PaymentRequest, Payment>().ReverseMap();
            CreateMap<PaymentResponseDto, Payment>().ReverseMap();
            CreateMap<PaymentResponse, Payment>().ReverseMap();
            CreateMap<PaymentResponse, PaymentResponseDto>();
            CreateMap<PaymentResponse, PaymentRequest>().ReverseMap();
        }
    }
}
