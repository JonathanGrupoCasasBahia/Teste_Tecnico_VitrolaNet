using ApplicationApp.Interfaces;
using ApplicationApp.OpenApp;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Configuration;
using Infrastructure.Interfaces;
using Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);


//Configuration
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

//GeneroMusical
builder.Services.AddSingleton<IRepositoryGeneroMusical, RepositoryGeneroMusical>();
builder.Services.AddSingleton<IGeneroMusicalApp, GeneroMusicalApp>();
builder.Services.AddSingleton<IServiceGeneroMusical, ServiceGeneroMusical>();
builder.Services.AddSingleton<IRepositoryGeneroMusical>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new RepositoryGeneroMusical(connectionString);
});

//Artista
builder.Services.AddSingleton<IRepositoryArtista, RepositoryArtista>();
builder.Services.AddSingleton<IArtistaApp, ArtistaApp>();
builder.Services.AddSingleton<IServiceArtista, ServiceArtista>();
builder.Services.AddSingleton<IRepositoryArtista>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new RepositoryArtista(connectionString);
});

//Album
builder.Services.AddSingleton<IRepositoryAlbum, RepositoryAlbum>();
builder.Services.AddSingleton<IAlbumApp, AlbumApp>();
builder.Services.AddSingleton<IServiceAlbum, ServiceAlbum>();
builder.Services.AddSingleton<IRepositoryAlbum>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new RepositoryAlbum(connectionString);
});

//Musica
builder.Services.AddSingleton<IRepositoryMusica, RepositoryMusica>();
builder.Services.AddSingleton<IMusicaApp, MusicaApp>();
builder.Services.AddSingleton<IServiceMusica, ServiceMusica>();
builder.Services.AddSingleton<IRepositoryMusica>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new RepositoryMusica(connectionString);
});


// criar instancia da ContextBase e initializar o database
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
