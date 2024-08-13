using PhotosiOrders.Dto;

namespace PhotosiOrders.Service;

public interface IOrderService
{
    Task<List<OrderDto>> GetAsync();
}