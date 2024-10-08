﻿using PhotosiOrders.Model;
using PhotosiOrders.Repository.Order;

namespace PhotosiOrders.xUnitTest.Repository;

public class OrderRepositoryTest : TestSetup
{
    [Fact]
    public async Task GetAllAndIncludeAsync_ShouldReturnList_Always()
    {
        // Arrange
        var repository = GetRepository();

        var orders = Enumerable.Range(0, _faker.Int(10, 30))
            .Select(_ => GenerateOrderAndSave())
            .ToList();
        
        // Act
        var result = await repository.GetAllAndIncludeAsync();
        
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(result.Count, orders.Count);
            Assert.Empty(result.Select(x => x.Id).Except(orders.Select(x => x.Id)));
            Assert.Empty(result.Select(x => x.OrderCode).Except(orders.Select(x => x.OrderCode)));
            Assert.Empty(result.Select(x => x.UserId).Except(orders.Select(x => x.UserId)));
            Assert.Empty(result.Select(x => x.AddressId).Except(orders.Select(x => x.AddressId)));
            
            Assert.Equal(result.Sum(x => x.OrderProducts.Count), result.Sum(x => x.OrderProducts.Count));
            
            // Campi univoci
            Assert.Equal(result.Select(x => x.Id).Distinct().Count(), result.Count);
            Assert.Equal(result.Select(x => x.OrderCode).Distinct().Count(), result.Count);
            
            // Campi obbligatori
            Assert.All(result, x => Assert.True(x.UserId > 0));
            Assert.All(result, x => Assert.True(x.AddressId > 0));
        });
    }

    [Fact]
    public async Task GetByIdAndIncludeAsync_ShouldReturnNull_IfObjectNotFound()
    {
        // Arrange
        var repository = GetRepository();
        
        // Act
        var result = await repository.GetByIdAndIncludeAsync(_faker.Int());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetByIdAndIncludeAsync_ShouldReturnObject_IfObjectFound()
    {
        // Arrange
        var repository = GetRepository();
        var order = GenerateOrderAndSave();
        
        // Act
        var result = await repository.GetByIdAndIncludeAsync(order.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, order.Id);
        Assert.Equal(result.OrderProducts.Count, order.OrderProducts.Count);
    }

    [Fact]
    public async Task GetAllAndIncludeByUserIdAsync_ShouldReturnList_Always()
    {
        // Arrange
        var repository = GetRepository();

        var userId = _faker.Int(1);
        var orders = Enumerable.Range(0, _faker.Int(10, 30))
            .Select(_ => GenerateOrderAndSave(userId))
            .ToList();
        
        // Act
        var result = await repository.GetAllAndIncludeByUserIdAsync(userId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(result.Count, orders.Count);
            Assert.True(result.TrueForAll(x => x.UserId == userId));
            Assert.Empty(result.Select(x => x.UserId).Except(orders.Select(x => x.UserId)));
        });
    }
    
    private IOrderRepository GetRepository() => new OrderRepository(_context);
    
    private Order GenerateOrderAndSave(int? userId = null)
    {
        var order = new Order()
        {
            OrderCode = _faker.Int(1),
            UserId = userId ?? _faker.Int(1),
            AddressId = _faker.Int(1),
            OrderProducts = Enumerable.Range(0, _faker.Int(10, 30))
                .Select(_ => GenerateOrderProducts())
                .ToList()
        };

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