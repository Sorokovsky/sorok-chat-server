using SorokChatServer.Domain.Options;
using SorokChatServer.Domain.Repositories;
using SorokChatServer.Domain.Services;
using SorokChatServer.Persistence.Postgres;
using SorokChatServer.Persistence.Postgres.Repositories;

namespace SorokChatServer.Application;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddConfigs();

        builder.Services.AddDatabase();
        builder.Services.AddServices();

        builder.Services.AddControllers();

        builder.Services.AddSwagger();

        builder.Services.AddMapper();

        var app = builder.Build();

        app.MapSwagger();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(x =>
        {
            x.AddMaps(typeof(PostgresContext).Assembly);
            x.AddMaps(typeof(IPasswordHasherService).Assembly);
            x.AddMaps(typeof(Program).Assembly);
        });
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen();
        services.AddOpenApi();
    }

    private static void MapSwagger(this WebApplication app)
    {
        app.MapOpenApi();
        app.UseSwaggerUI();
        app.UseSwagger();
    }

    private static void AddDatabase(this IServiceCollection services)
    {
        services.AddTransient<PostgresContext>();
        services.AddTransient<IUsersRepository, UsersRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUsersService, UsersService>();
        services.AddSingleton<IPasswordHasherService, Argon2PasswordHasher>();
        services.AddSingleton<IAccessTokenStorage, BearerTokenStorage>();
        services.AddSingleton<IRefreshTokenStorage, CookieTokenStorage>();
        services.AddSingleton<ITokenSerializer, JwtTokenService>();
        services.AddSingleton<ITokenDeserializer, JwtTokenService>();
    }

    private static void AddConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
    }
}