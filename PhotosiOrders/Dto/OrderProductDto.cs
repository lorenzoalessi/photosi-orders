using System.Diagnostics.CodeAnalysis;

namespace PhotosiOrders.Dto;

[ExcludeFromCodeCoverage]
public class OrderProductDto
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
}