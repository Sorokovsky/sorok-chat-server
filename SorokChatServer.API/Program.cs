using SorokChatServer.Core.Configurations;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Middlewares;
using SorokChatServer.Core.Options;
using SorokChatServer.Core.Services;
using SorokChatServer.DataAccess;
using SorokChatServer.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<Database>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IFilesService, FilesService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddSingleton<FilesConfiguration>();
builder.Services.AddScoped<ErrorHandlerMiddleware>();
builder.Services.Configure<HashingOptions>(config.GetSection(HashingOptions.Hashing));
builder.Services.Configure<FilesOptions>(config.GetSection(FilesOptions.Files));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

var filesConfiguration = app.Services.GetRequiredService<FilesConfiguration>();
app.UseStaticFiles(filesConfiguration);

app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyMethod();
    x.AllowAnyHeader();
});

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();