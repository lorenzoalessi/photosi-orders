using AutoMapper;
using Bogus;
using Moq;
using PhotosiOrders.Dto;
using PhotosiOrders.Exceptions;
using PhotosiOrders.Mapper;
using PhotosiOrders.Model;
using PhotosiOrders.Repository.Order;
using PhotosiOrders.Service;

namespace PhotosiOrders.xUnitTest.Service;

public class OrderServiceTest : TestSetup
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly IMapper _mapper;

    public OrderServiceTest()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();

        var config = new MapperConfiguration(conf =>
        {
            conf.AddProfile(typeof(OrderMapperProfile));
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetAllAndIncludeAsync_ShouldReturnList_Always()
    {
        // Arrange
        var service = GetService();

        var orders = Enumerable.Range(0, _faker.Int(10, 30))
            .Select(_ => GenerateOrder())
            .ToList();
        
        // Setup mock del repository
        _mockOrderRepository.Setup(x => x.GetAllAndIncludeAsync())
            .ReturnsAsync(orders);
        
        // Act
        var result = await service.GetAsync();
        
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
        
        _mockOrderRepository.Verify(x => x.GetAllAndIncludeAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_IfNoObjectFounded()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int();
        
        // Act
        var result = await service.GetByIdAsync(input);

        // Assert
        Assert.Null(result);
        _mockOrderRepository.Verify(x => x.GetByIdAndIncludeAsync(input), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnObject_IfObjectFounded()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int();

        var order = GenerateOrder();
        
        _mockOrderRepository.Setup(x => x.GetByIdAndIncludeAsync(input))
            .ReturnsAsync(order);
        
        // Act
        var result = await service.GetByIdAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
        Assert.Equal(order.OrderCode, result.OrderCode);
        Assert.Equal(order.AddressId, result.AddressId);
        Assert.Equal(order.UserId, result.UserId);
        Assert.Equal(order.OrderProducts.Count, result.OrderProducts.Count);
        _mockOrderRepository.Verify(x => x.GetByIdAndIncludeAsync(input), Times.Once);
    }

    [Fact]
    public void UpdateAsync_ShouldThrowException_IfObjectNotFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var orderDto = GenerateOrderDto();
        
        // Act
        Assert.ThrowsAsync<OrderException>(async () => await service.UpdateAsync(id, orderDto));

        // Assert
        _mockOrderRepository.Verify(x => x.GetByIdAndIncludeAsync(id), Times.Once);
        _mockOrderRepository.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdate_IfObjectFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var orderDto = GenerateOrderDto(id);
        var order = GenerateOrder(id);
        
        _mockOrderRepository.Setup(x => x.GetByIdAndIncludeAsync(id))
            .ReturnsAsync(order);
        
        // Act
        var result = await service.UpdateAsync(id, orderDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.AddressId, orderDto.AddressId);
        Assert.Equal(result.OrderProducts.Count, orderDto.OrderProducts.Count);
        Assert.True(!result.OrderProducts.Select(x => x.Id).Except(orderDto.OrderProducts.Select(y => y.Id)).Any());
        
        _mockOrderRepository.Verify(x => x.GetByIdAndIncludeAsync(id), Times.Once);
        _mockOrderRepository.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteAsync_ShouldReturnTrueOrFalse_IfAddressBookFoundedOrNot(bool founded)
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int();

        _mockOrderRepository.Setup(x => x.DeleteAsync(input))
            .ReturnsAsync(founded);
        
        // Act
        var result = await service.DeleteAsync(input);

        // Assert
        _mockOrderRepository.Verify(x => x.DeleteAsync(input), Times.Once);
        Assert.Equal(result, founded);
    }

    [Fact]
    public async Task AddAsync_ShouldAddObject_Always()
    {
        // Arrange
        var service = GetService();
        var input = GenerateOrderDto();
        
        // Act
        var result = await service.AddAsync(input);
        
        // Assert
        _mockOrderRepository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(result.AddressId, input.AddressId);
        Assert.Equal(result.OrderProducts.Count, input.OrderProducts.Count);
        Assert.Equal(result.OrderCode, input.OrderCode);
        Assert.Equal(result.UserId, input.UserId);
    }
    
    private IOrderService GetService() => new OrderService(_mockOrderRepository.Object, _mapper);
    
    private OrderDto GenerateOrderDto(int? id = null)
    {
        return new OrderDto()
        {
            Id = id ?? _faker.Int(1),
            OrderCode = _faker.Int(1),
            UserId = _faker.Int(1),
            AddressId = _faker.Int(1),
            OrderProducts = Enumerable.Range(0, _faker.Int(10, 30))
                .Select(_ => GenerateOrderProductsDto())
                .ToList()
        };
    }

    private OrderProductDto GenerateOrderProductsDto()
    {
        return new OrderProductDto()
        {
            Quantity = _faker.Int(1)
        };
    }
    
    private Order GenerateOrder(int? id = null)
    {
        return new Order()
        {
            Id = id ?? _faker.Int(1),
            OrderCode = _faker.Int(1),
            UserId = _faker.Int(1),
            AddressId = _faker.Int(1),
            OrderProducts = Enumerable.Range(0, _faker.Int(10, 30))
                .Select(_ => GenerateOrderProducts())
                .ToList()
        };
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