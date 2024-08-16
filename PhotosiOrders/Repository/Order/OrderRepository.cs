using Microsoft.EntityFrameworkCore;
using PhotosiOrders.Model;

namespace PhotosiOrders.Repository.Order;

public class OrderRepository : GenericRepository<Model.Order>, IOrderRepository
{
    public OrderRepository(Context context) : base(context)
    {
    }

    public async Task<List<Model.Order>> GetAllAndIncludeAsync()
    {
        return await _context.Orders
            .Include(x => x.OrderProducts)
            .ToListAsync();
    }

    public async Task<Model.Order?> GetByIdAndIncludeAsync(int id)
    {
        return await _context.Orders
            .Include(x => x.OrderProducts)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}