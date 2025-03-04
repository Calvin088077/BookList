using Microsoft.EntityFrameworkCore;
using myList.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        var version = new MySqlServerVersion(new Version(9, 1, 0));
        await WaitForDatabaseAsync(connectionString, version);
        var bookController = host.Services.GetRequiredService<IController<book>>();
        var consoleUI = new ConsoleUI(bookController);

        Console.WriteLine("歡迎來到備忘書單");
        await consoleUI.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                var Version = new MySqlServerVersion(new Version(9, 1, 0));

                services.AddDbContext<listContext>(options =>
                    options.UseMySql(connectionString, Version)
                );
                services.AddLogging(options => 
                    options.SetMinimumLevel(LogLevel.None));

                services.AddScoped<IController<book>, BookController>();
            });

    private static async Task WaitForDatabaseAsync(string connectionString, MySqlServerVersion version)
    {
        var retries = 5;
        var delay = TimeSpan.FromSeconds(5);

        while (retries > 0)
        {
            try
            {
                using var context = new listContext(new DbContextOptionsBuilder<listContext>()
                    .UseMySql(connectionString, version)
                    .Options);

                await context.Database.OpenConnectionAsync();
                break; 
            }
            catch (Exception)
            {
                retries--;
                Console.WriteLine("等待資料庫啟動中...");
                await Task.Delay(delay);
            }
        }

        if (retries == 0)
        {
            Console.WriteLine("無法連接到資料庫，請檢查MySQL服務是否啟動。");
            Environment.Exit(1);
        }
    }
}



