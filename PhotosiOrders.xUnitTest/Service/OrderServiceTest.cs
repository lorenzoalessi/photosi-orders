using AutoMapper;
using Moq;
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
    
    private IOrderService GetService() => new OrderService(_mockOrderRepository.Object, _mapper);
    
    private Order GenerateOrder()
    {
        return new Order()
        {
            Id = _faker.Int(1),
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