using ApplicationApp.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebGeneroMusical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumApp _IAlbumApp;

        public AlbumController(IAlbumApp IAlbumApp)
        {
            _IAlbumApp = IAlbumApp;
        }

        [HttpGet("/api/ListarAlbuns")]
        [Produces("application/json")]
        public async Task<List<Album>> ListarAlbuns()
        {
            return await _IAlbumApp.List();
        }

        [HttpGet("/api/GetByIdAlbum")]
        [Produces("application/json")]
        public async Task<Album> GetByIdAlbum(int Id)
        {
            return await _IAlbumApp.GetEntityByID(Id);
        }

        [HttpGet("/api/GetByNameAlbum")]
        [Produces("application/json")]
        public async Task<List<Album>> GetByNameAlbum(string NomeAlbum)
        {
            return await _IAlbumApp.GetEntityByName(NomeAlbum);
        }

        [HttpPost("/api/AdicionarAlbum")]
        [Produces("application/json")]
        public async Task AdiconarAlbum(string NomeAlbum, int AnoLancamento, int IdArtista)
        {
            await _IAlbumApp.Add(NomeAlbum, AnoLancamento, IdArtista);
        }

        [HttpPut("/api/AtualizarAlbum")]
        [Produces("application/json")]
        public async Task AtualizarAlbum(int IdAlbum, string NovoNomeAlbum, int NovoAnoLancamento, int NovoIdArtista)
        {
            await _IAlbumApp.Update(IdAlbum, NovoNomeAlbum, NovoAnoLancamento, NovoIdArtista);
        }

        [HttpDelete("/api/ApagarAlbum")]
        [Produces("application/json")]
        public async Task ApagarAlbum(int Id)
        {
            await _IAlbumApp.Delete(Id);
        }

    }
}
