using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RDesigner.Configuration;
using RDesigner.Services;
using RDesigner.ViewModels;
using RDesigner.Views;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Username), "Database:Username is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Password), "Database:Password is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.DBName), "Database:DBName is required.")
            .Validate(options => options.Port > 0, "Database:Port must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton(provider =>
        {
            var databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            return Npgsql.NpgsqlDataSource.Create(databaseOptions.CreateConnectionString());
        });

        services.AddSingleton<IDBService, PostgresDBService>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainView>(provider => new MainView(provider));

        services.AddTransient<MainWindow>();
    }
}
