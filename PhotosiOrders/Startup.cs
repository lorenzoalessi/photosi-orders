using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PhotosiOrders.Model;
using PhotosiOrders.Repository.Order;
using PhotosiOrders.Service;

namespace PhotosiOrders;

[ExcludeFromCodeCoverage]
public class Startup
{
    private readonly WebApplicationBuilder _builder;

    public Startup(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public async Task ConfigureServices()
    {
        _ = _builder.Services.AddControllers();

        _builder.Services.AddAutoMapper(typeof(Startup));
        // Aggiungo servizi e repository al container di dependency injection.
        ConfigureServices(_builder.Services);
        ConfigureRepositories(_builder.Services);

        await ConfigureDb();
    }

    public void Configure(IApplicationBuilder app)
    {
        _ = app.UseRouting();
        _ = app.UseAuthorization();
        _ = app.UseEndpoints(x => x.MapControllers());
    }

    private async Task ConfigureDb()
    {
        _ = _builder.Services.AddDbContext<Context>(options =>
            options.UseNpgsql(_builder.Configuration.GetConnectionString("PostgreSql"))
        );

        await using var serviceProvider = _builder.Services.BuildServiceProvider();
        // Applico le migrazioni
        await serviceProvider.GetRequiredService<Context>().Database.MigrateAsync();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddScoped<IOrderService, OrderService>();
    }

    private void ConfigureRepositories(IServiceCollection services)
    {
        _ = services.AddScoped<IOrderRepository, OrderRepository>();
    }
}