﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SorokChatServer.Core.Entities;

namespace SorokChatServer.DataAccess;

public class Database : DbContext
{
    private readonly IConfiguration _configuration;

    public Database(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseNpgsql(_configuration.GetConnectionString(nameof(Database)))
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Database).Assembly);
    }
}