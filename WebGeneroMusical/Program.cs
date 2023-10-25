using ApplicationApp.Interfaces;
using ApplicationApp.OpenApp;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Configuration;
using Infrastructure.Interfaces;
using Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();


// Obtém a cadeia de conexão do arquivo de configuração
var connectionString = configuration.GetConnectionString("DefaultConnection");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(_ => connectionString);
builder.Services.AddSingleton<IRepositoryGeneroMusical, RepositoryGeneroMusical>();
builder.Services.AddSingleton<IGeneroMusicalApp, GeneroMusicalApp>();
builder.Services.AddSingleton<IServiceGeneroMusical, ServiceGeneroMusical>();

builder.Services.AddSingleton<IRepositoryGeneroMusical>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new RepositoryGeneroMusical(connectionString);
});

// Create an instance of ContextBase and initialize the database
var contextBase = new ContextBase(connectionString);
contextBase.InicializaDataBase();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
