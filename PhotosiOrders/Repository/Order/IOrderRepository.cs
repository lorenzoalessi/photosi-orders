namespace PhotosiOrders.Repository.Order;

public interface IOrderRepository : IGenericRepository<Model.Order>
{
    Task<List<Model.Order>> GetAllAndIncludeAsync();
    
    Task<Model.Order?> GetByIdAndIncludeAsync(int id);
}