using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SorokChatServer.Persistence.Postgres;

public class PostgresContext : DbContext
{
    private readonly IConfiguration _configuration;

    public PostgresContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSnakeCaseNamingConvention()
            .UseNpgsql(_configuration.GetConnectionString(nameof(PostgresContext)));
    }
}