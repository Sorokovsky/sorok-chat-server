using SorokChatServer.Application.Conventions;
using SorokChatServer.Core.Options;
using SorokChatServer.Core.Services;
using SorokChatServer.Logic.Services;
using SorokChatServer.Postgres;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddMvcOptions(options => { options.Conventions.Add(new KebabCaseRouteConvention()); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ArgonOptions>(builder.Configuration.GetSection(nameof(ArgonOptions)));
builder.Services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<DatabaseContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();