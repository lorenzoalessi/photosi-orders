using Microsoft.AspNetCore.Mvc;
using PhotosiOrders.Dto;
using PhotosiOrders.Exceptions;
using PhotosiOrders.Service;

namespace PhotosiOrders.Controllers;

[Route("api/v1/orders")]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        return Ok(await _orderService.GetByIdAsync(id));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderDto orderDto)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        // Valido la dto di richiesta
        var errorMessage = ValidateOrderDto(orderDto);
        if (!string.IsNullOrEmpty(errorMessage))
            return BadRequest(errorMessage);

        try
        {
            var result = await _orderService.UpdateAsync(id, orderDto);
            return Ok($"Ordine con ID {result.Id} salvato con successo");
        }
        catch (OrderException e)
        {
            return BadRequest($"Errore nella richiesta di aggiornamento: {e.Message}");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] OrderDto orderDto)
    {
        // Valido la dto di richiesta
        var errorMessage = ValidateOrderDto(orderDto);
        if (!string.IsNullOrEmpty(errorMessage))
            return BadRequest(errorMessage);

        try
        {
            return Ok(await _orderService.AddAsync(orderDto));
        }
        catch (OrderException e)
        {
            return BadRequest($"Errore nella richiesta di inserimento: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        try
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted)
                return StatusCode(500, "Errore nella richiesta di eliminazione");
            
            return Ok($"Ordine con ID {id} eliminato con successo");
        }
        catch (OrderException e)
        {
            return BadRequest($"Errore nella richiesta di eliminazione: {e.Message}");
        }
    }

    private string ValidateOrderDto(OrderDto orderDto)
    {
        if (orderDto.UserId < 1)
            return "ID utente fornito non valido";

        if (orderDto.AddressId < 1)
            return "ID indirizzo fornito non valido";

        if (orderDto.OrderProducts.Count < 1)
            return "Nessun prodotto specificato per l'ordine";

        return string.Empty;
    }
}