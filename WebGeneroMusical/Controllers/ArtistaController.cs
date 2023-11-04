using ApplicationApp.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebGeneroMusical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        private readonly IArtistaApp _IArtistaApp;

        public ArtistaController(IArtistaApp IArtistaApp)
        {
            _IArtistaApp = IArtistaApp;
        }

        [HttpGet("/api/ListarArtistas")]
        [Produces("application/json")]
        public async Task<List<Artista>> ListarArtistas()
        {
            return await _IArtistaApp.List();
        }

        [HttpGet("/api/GetArtistaId")]
        [Produces("application/json")]
        public async Task<Artista> GetArtistaId(int Id)
        {
            return await _IArtistaApp.GetEntityByID(Id);
        }

        [HttpGet("/api/GetArtistaName")]
        [Produces("application/json")]
        public async Task<List<Artista>> GetArtistaName(string TrechoNome)
        {
            return await _IArtistaApp.GetEntityByName(TrechoNome);
        }

        [Produces("application/json")]
        [HttpPost("/api/AdicionarArtista")]
        public async Task AdicionarArtista(string NomeArtista, int IdGenero)
        {
            await _IArtistaApp.Add(NomeArtista, IdGenero);
        }

        [Produces("application/json")]
        [HttpPut("/api/AtualizarArtista")]
        public async Task AtualizarArtista(int id, string novoNomeArtista, int NovoIdGenero)
        {
            await _IArtistaApp.Update(id, novoNomeArtista,NovoIdGenero);
        }

        [Produces("application/json")]
        [HttpDelete("/api/DeletarArtista")]
        public async Task DeletarArtista(int id)
        {
            await _IArtistaApp.Delete(id);
        }

    }
}
