using AutoMapper;
using PhotosiOrders.Dto;
using PhotosiOrders.Exceptions;
using PhotosiOrders.Model;
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
        var user = await _orderRepository.GetAllAndIncludeAsync();
        return _mapper.Map<List<OrderDto>>(user);
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAndIncludeAsync(id);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> UpdateAsync(int id, OrderDto orderDto)
    {
        var order = await _orderRepository.GetByIdAndIncludeAsync(id);
        if (order == null)
            throw new OrderException($"L'ordine con ID {id} non esiste");
        
        // Non credo sia corretto poter modificare l'ID utente
        // in quanto l'ordine di un determinato utente non può essere "trasferito" ad un'altro
        // order.UserId = orderDto.UserId;
        
        // Stesso discorso per il codice dell'ordine
        // order.OrderCode = orderDto.OrderCode;
        
        order.AddressId = orderDto.AddressId;
        order.OrderProducts = orderDto.OrderProducts.Select(product => new OrderProduct()
        {
            ProductId = product.Id,
            Quantity = product.Quantity
        }).ToList();

        await _orderRepository.SaveAsync();

        return orderDto;
    }

    public async Task<OrderDto> AddAsync(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.AddAsync(order);
        
        // Aggiorno l'Id della dto senza rimappare
        orderDto.Id = order.Id;
        return orderDto;
    }

    public async Task<bool> DeleteAsync(int id) => await _orderRepository.DeleteAsync(id);
}