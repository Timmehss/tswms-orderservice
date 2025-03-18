using AutoMapper;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Api.MappingProfiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
