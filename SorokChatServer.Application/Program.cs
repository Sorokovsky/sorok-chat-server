using Microsoft.AspNetCore.Authentication;
using SorokChatServer.Application.Conventions;
using SorokChatServer.Application.Hubs;
using SorokChatServer.Core.Handlers;
using SorokChatServer.Core.Options;
using SorokChatServer.Core.Services;
using SorokChatServer.Logic.Repositories;
using SorokChatServer.Logic.Services;
using SorokChatServer.Postgres;
using SorokChatServer.Postgres.Repositories;
using AuthenticationService = SorokChatServer.Core.Services.AuthenticationService;
using IAuthenticationService = SorokChatServer.Logic.Services.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddMvcOptions(options => { options.Conventions.Add(new KebabCaseRouteConvention()); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ArgonOptions>(builder.Configuration.GetSection(nameof(ArgonOptions)));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<DiffieHellmanOptions>(builder.Configuration.GetSection(nameof(DiffieHellmanOptions)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(nameof(JwtHandler))
    .AddScheme<AuthenticationSchemeOptions, JwtHandler>(nameof(JwtHandler), null);
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<ITokenSerializerService, JwtSerializerService>();
builder.Services.AddSingleton<IAccessTokenStorage, AccessTokenStorage>();
builder.Services.AddSingleton<IRefreshTokenStorage, RefreshTokenStorage>();
builder.Services.AddSingleton<IDiffieHellmanService, DiffieHellmanService>();
builder.Services.AddSingleton<ITripleDiffieHellmanService, TripleDiffieHellmanService>();
builder.Services.AddSingleton<IKeyDerivationFunction, Sha256DerivationFunction>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();
builder.Services.AddScoped<IChatsRepository, ChatsRepository>();
builder.Services.AddScoped<IChatsService, ChatsService>();
builder.Services.AddSignalR()
    .AddHubOptions<ChatsHub>(options => { options.EnableDetailedErrors = true; })
    .AddMessagePackProtocol();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllPermit", corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:4200");
        corsPolicyBuilder.AllowAnyMethod();
        corsPolicyBuilder.AllowAnyHeader();
        corsPolicyBuilder.AllowCredentials();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllPermit");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatsHub>("/messages").RequireAuthorization();

app.MapControllers();

app.Run();