using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosiOrders.Model;

[Table("order")]
public class Order
{
    [Column("id"), Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("order_code"), Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderCode { get; set; }

    [Column("user_id"), Required]
    public int UserId { get; set; }

    [Column("address_id"), Required]
    public int AddressId { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
}