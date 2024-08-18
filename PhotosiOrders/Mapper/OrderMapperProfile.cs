using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using PhotosiOrders.Dto;
using PhotosiOrders.Model;

namespace PhotosiOrders.Mapper;

[ExcludeFromCodeCoverage]
public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(x => x.OrderProducts, y => y.MapFrom(z => z.OrderProducts))
            .ReverseMap();

        CreateMap<OrderProduct, OrderProductDto>()
            .ForMember(x => x.Id, y => y.MapFrom(z => z.ProductId))
            .ForMember(x => x.Quantity, y => y.MapFrom(z => z.Quantity))
            .ReverseMap();
    }
}