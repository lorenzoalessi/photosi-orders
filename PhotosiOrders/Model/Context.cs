using Microsoft.EntityFrameworkCore;

namespace PhotosiOrders.Model;

public class Context : DbContext
{
    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public Context()
    {
    }

    public Context(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderProduct>()
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderProducts)
            .HasForeignKey(x => x.OrderId)
            ;

        modelBuilder.Entity<Order>()
            .HasIndex(x => x.OrderCode)
            .IsUnique()
            ;
    }
}