using ApplicationApp.Interfaces;
using ApplicationApp.OpenApp;
using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebGeneroMusical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        private readonly IMusicaApp _IMusicaApp;

        public MusicaController(IMusicaApp IMusicaApp)
        {
            _IMusicaApp = IMusicaApp;
        }

        [HttpGet("/api/ListarMusicas")]
        [Produces("application/json")]
        public async Task<List<Musica>> ListarMusicas()
        {
            return await _IMusicaApp.List();
        }

        [HttpGet("/api/GetMusicaId")]
        [Produces("application/json")]
        public async Task<Musica> GetMusicaId(int Id)
        {
            return await _IMusicaApp.GetEntityByID(Id);
        }

        [HttpGet("/api/GetMusicaName")]
        [Produces("application/json")]
        public async Task<List<Musica>> GetMusicaName(string TrechoNome)
        {
            return await _IMusicaApp.GetEntityByName(TrechoNome);
        }

        [Produces("application/json")]
        [HttpPost("/api/AdicionarMusica")]
        public async Task AdicionarMusica(string NomeMusica, int Ordem, int IdAlbum)
        {
            await _IMusicaApp.Add(NomeMusica, Ordem,  IdAlbum);
        }

        [Produces("application/json")]
        [HttpPut("/api/AtualizarMusica")]
        public async Task AtualizarMusica(int id, string novoNomeMusica, int NovaOrdem, int NovoIdAlbum)
        {
            await _IMusicaApp.Update(id, novoNomeMusica, NovaOrdem, NovoIdAlbum);
        }

        [Produces("application/json")]
        [HttpDelete("/api/DeletarMusica")]
        public async Task DeletarArtista(int id, int IdAlbum)
        {
            await _IMusicaApp.Delete(id, IdAlbum);
        }


    }
}
