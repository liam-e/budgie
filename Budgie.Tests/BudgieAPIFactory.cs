using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.TestHost;
using Budgie.API.Database;
using Budgie.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Budgie.Tests;

public class BudgieAPIFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<BudgetContext>));
            services.AddSingleton<DbConnectionProvider>(_ => new DbConnectionProvider(_dbContainer.GetConnectionString()));
            services.AddScoped<DbConnection>(provider =>
            {
                var dbConnectionProvider = provider.GetRequiredService<DbConnectionProvider>();
                return dbConnectionProvider.GetConnection();
            });

            services.AddDbContext<BudgetContext>((serviceProvider, options) =>
            {
                var connection = serviceProvider.GetRequiredService<DbConnection>();
                options.UseNpgsql(connection);
            });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
                context.Database.Migrate();
            }
        });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.DisposeAsync().AsTask();
    }
}
