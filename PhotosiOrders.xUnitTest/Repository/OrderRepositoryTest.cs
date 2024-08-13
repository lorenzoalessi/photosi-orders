using PhotosiOrders.Model;
using PhotosiOrders.Repository.Order;

namespace PhotosiOrders.xUnitTest.Repository;

public class OrderRepositoryTest : TestSetup
{
    [Fact]
    public async Task GetAsync_ShouldReturnList_Always()
    {
        // Arrange
        var repository = GetRepository();

        var orders = Enumerable.Range(0, _faker.Int(10, 30))
            .Select(_ => GenerateOrderAndSave())
            .ToList();
        
        // Act
        var result = await repository.GetAsync();
        
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(result.Count, orders.Count);
            Assert.Empty(result.Select(x => x.Id).Except(orders.Select(x => x.Id)));
            Assert.Empty(result.Select(x => x.OrderCode).Except(orders.Select(x => x.OrderCode)));
            Assert.Empty(result.Select(x => x.UserId).Except(orders.Select(x => x.UserId)));
            Assert.Empty(result.Select(x => x.AddressId).Except(orders.Select(x => x.AddressId)));
            
            // Campi univoci
            Assert.Equal(result.Select(x => x.Id).Distinct().Count(), result.Count);
            Assert.Equal(result.Select(x => x.OrderCode).Distinct().Count(), result.Count);
            
            // Campi obbligatori
            Assert.All(result, x => Assert.True(x.UserId > 0));
            Assert.All(result, x => Assert.True(x.AddressId > 0));
        });
    }
    
    private IOrderRepository GetRepository() => new OrderRepository(_context);
    
    private Order GenerateOrderAndSave()
    {
        var order = new Order()
        {
            Id = _faker.Int(1),
            OrderCode = _faker.Int(1),
            UserId = _faker.Int(1),
            AddressId = _faker.Int(1),
            OrderProducts = new List<OrderProduct>() // Lista vuota, la GetAsync non fa la include
        };

        _context.Add(order);
        _context.SaveChanges();

        return order;
    }
}