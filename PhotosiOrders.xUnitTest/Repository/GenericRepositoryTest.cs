using PhotosiOrders.Model;
using PhotosiOrders.Repository;

namespace PhotosiOrders.xUnitTest.Repository;

public class GenericRepositoryTest : TestSetup
{
    [Fact]
    public async Task AddAsync_Should_AddObject_Always()
    {
        // Arrange
        var repository = GetRepository();
        var input = GenerateOrder();
        
        // Act
        var result = await repository.AddAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(input.AddressId, result.AddressId);
        Assert.Equal(input.OrderProducts.Count, result.OrderProducts.Count);
        Assert.Equal(input.OrderCode, result.OrderCode);
        Assert.Equal(input.UserId, result.UserId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfObjectNotFound()
    {
        // Arrange
        var repository = GetRepository();
        
        // Act
        var result = await repository.DeleteAsync(_faker.Int(1));
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfObjectFound()
    {
        // Arrange
        var repository = GetRepository();
        var addressBook = GenerateOrderAndSave();
        
        // Act
        var result = await repository.DeleteAsync(addressBook.Id);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SaveAsync_ShouldNotThrowException_Always()
    {
        // Arrange
        var repository = GetRepository();

        var input = GenerateOrder();
        await _context.AddAsync(input);

        // Act
        await repository.SaveAsync();
        
        // Assert
        Assert.True(input.Id > 0);
    }
    
    private IGenericRepository<Order> GetRepository() => new GenericRepository<Order>(_context);

    private Order GenerateOrder()
    {
        return new Order()
        {
            OrderCode = _faker.Int(1),
            UserId = _faker.Int(1),
            AddressId = _faker.Int(1),
            OrderProducts = Enumerable.Range(0, _faker.Int(10, 30))
                .Select(_ => GenerateOrderProducts())
                .ToList()
        };
    }
    
    private Order GenerateOrderAndSave()
    {
        var order = GenerateOrder();

        _context.Add(order);
        _context.SaveChanges();

        return order;
    }

    private OrderProduct GenerateOrderProducts()
    {
        return new OrderProduct()
        {
            ProductId = _faker.Int(1),
            Quantity = _faker.Int(1)
        };
    }
}