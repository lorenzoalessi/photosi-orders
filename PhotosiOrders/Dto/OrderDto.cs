﻿using System.Diagnostics.CodeAnalysis;

namespace PhotosiOrders.Dto;

[ExcludeFromCodeCoverage]
public class OrderDto
{
    public int Id { get; set; }

    public int OrderCode { get; set; }

    public int UserId { get; set; }

    public int AddressId { get; set; }
    
    public List<OrderProductDto> OrderProducts { get; set; }
}