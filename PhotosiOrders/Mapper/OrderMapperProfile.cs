using AutoMapper;
using PhotosiOrders.Dto;
using PhotosiOrders.Model;

namespace PhotosiOrders.Mapper;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<Order, OrderDto>()
            .ReverseMap();

        CreateMap<OrderProduct, OrderProductDto>()
            .ReverseMap();
    }
}