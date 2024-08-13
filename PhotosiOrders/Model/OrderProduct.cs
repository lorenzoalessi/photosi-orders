using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosiOrders.Model;

[Table("order_products")]
public class OrderProduct
{
    [Column("id"), Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("product_id"), Required]
    public int ProductId { get; set; }

    [Column("order_id"), Required]
    public int OrderId { get; set; }

    [Column("quantity"), Required]
    public int Quantity { get; set; }

    public virtual Order Order { get; set; }
}