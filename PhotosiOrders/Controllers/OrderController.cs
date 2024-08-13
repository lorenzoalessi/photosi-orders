using Microsoft.AspNetCore.Mvc;
using PhotosiOrders.Service;

namespace PhotosiOrders.Controllers;

[Route("api/v1/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _orderService.GetAsync());
}