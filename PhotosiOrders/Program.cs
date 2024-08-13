namespace PhotosiOrders;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        Startup startup = new Startup(builder);
        await startup.ConfigureServices();

        WebApplication app = builder.Build();

        startup.Configure(app);

        app.Run();
    }
}