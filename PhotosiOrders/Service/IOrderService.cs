using PhotosiOrders.Dto;

namespace PhotosiOrders.Service;

public interface IOrderService
{
    Task<List<OrderDto>> GetAsync();
    
    Task<OrderDto> GetByIdAsync(int id);

    Task<OrderDto> UpdateAsync(int id, OrderDto orderDto);
    
    Task<OrderDto> AddAsync(OrderDto orderDto);
    
    Task<bool> DeleteAsync(int id);
    
}