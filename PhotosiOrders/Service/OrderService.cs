using AutoMapper;
using PhotosiOrders.Dto;
using PhotosiOrders.Repository.Order;

namespace PhotosiOrders.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<List<OrderDto>> GetAsync()
    {
        var user = await _orderRepository.GetAsync();
        return _mapper.Map<List<OrderDto>>(user);
    }
}