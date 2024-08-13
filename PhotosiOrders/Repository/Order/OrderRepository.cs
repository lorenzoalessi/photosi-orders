using PhotosiOrders.Model;

namespace PhotosiOrders.Repository.Order;

public class OrderRepository : GenericRepository<Model.Order>, IOrderRepository
{
    public OrderRepository(Context context) : base(context)
    {
    }
}