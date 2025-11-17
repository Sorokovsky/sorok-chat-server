using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SorokChatServer.Logic.Entities;
using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Postgres;

public class DatabaseContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<MessageEntity> Messages => Set<MessageEntity>();
    public DbSet<ChatEntity> Chats => Set<ChatEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString(nameof(DatabaseContext));
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(nameof(connectionString));

        optionsBuilder.UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}