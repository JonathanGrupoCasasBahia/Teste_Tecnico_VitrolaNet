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

var connectionString = configuration.GetConnectionString("DefaultConnection");

// criar instancia da ContextBase e initializar o database
var contextBase = new ContextBase(connectionString);
contextBase.InicializaDataBase();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(c => connectionString);

//GeneroMusical
builder.Services.AddSingleton<IRepositoryGeneroMusical, RepositoryGeneroMusical>();
builder.Services.AddSingleton<IGeneroMusicalApp, GeneroMusicalApp>();
builder.Services.AddSingleton<IServiceGeneroMusical, ServiceGeneroMusical>();


//Artista
builder.Services.AddSingleton<IRepositoryArtista, RepositoryArtista>();
builder.Services.AddSingleton<IArtistaApp, ArtistaApp>();
builder.Services.AddSingleton<IServiceArtista, ServiceArtista>();

//Album
builder.Services.AddSingleton<IRepositoryAlbum, RepositoryAlbum>();
builder.Services.AddSingleton<IAlbumApp, AlbumApp>();
builder.Services.AddSingleton<IServiceAlbum, ServiceAlbum>();


//Musica
builder.Services.AddSingleton<IRepositoryMusica, RepositoryMusica>();
builder.Services.AddSingleton<IMusicaApp, MusicaApp>();
builder.Services.AddSingleton<IServiceMusica, ServiceMusica>();

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
