using Microsoft.EntityFrameworkCore;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres;

public class PostgresContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
}