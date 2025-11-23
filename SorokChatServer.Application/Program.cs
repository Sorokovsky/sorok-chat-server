using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(nameof(JwtHandler))
    .AddScheme<AuthenticationSchemeOptions, JwtHandler>(nameof(JwtHandler), null)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) && context.Request.Path.StartsWithSegments("/messages"))
                    context.Token = accessToken;

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<ITokenSerializerService, JwtSerializerService>();
builder.Services.AddSingleton<IAccessTokenStorage, AccessTokenStorage>();
builder.Services.AddSingleton<IRefreshTokenStorage, RefreshTokenStorage>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();
builder.Services.AddScoped<IChatsRepository, ChatsRepository>();
builder.Services.AddScoped<IChatsService, ChatsService>();
builder.Services.AddSignalR()
    .AddHubOptions<ChatsHub>(options => { options.EnableDetailedErrors = true; });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllPermit", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin();
        corsPolicyBuilder.AllowAnyMethod();
        corsPolicyBuilder.AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllPermit");
app.MapHub<ChatsHub>("/messages").RequireAuthorization();

app.MapControllers();

app.Run();