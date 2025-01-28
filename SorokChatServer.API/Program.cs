using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Options;
using SorokChatServer.Core.Services;
using SorokChatServer.DataAccess;
using SorokChatServer.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<Database>(ServiceLifetime.Transient);
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IFilesService, FilesService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.Configure<HashingOptions>(config.GetSection(HashingOptions.Hashing));
builder.Services.Configure<FilesOptions>(config.GetSection(FilesOptions.Files));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();