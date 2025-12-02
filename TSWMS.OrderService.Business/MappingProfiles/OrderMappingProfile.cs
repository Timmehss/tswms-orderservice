using AutoMapper;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Business.MappingProfiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<Order, CreateOrderDto>();
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<OrderItem, CreateOrderItemDto>();

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
