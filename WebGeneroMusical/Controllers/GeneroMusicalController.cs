using ApplicationApp.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebGeneroMusical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroMusicalController : ControllerBase
    {
        private readonly IGeneroMusicalApp _IGeneneroMusicalApp;

        public GeneroMusicalController(IGeneroMusicalApp IGeneneroMusicalApp)
        {
            _IGeneneroMusicalApp = IGeneneroMusicalApp;
        }

        [HttpGet("/api/ListarGeneroMusical")]
        [Produces("application/json")]
        public async Task<List<GeneroMusical>> ListarGeneroMusical ()
        {
            return await _IGeneneroMusicalApp.List();
        }

        [HttpGet("/api/GetGeneroMusical")]
        [Produces("application/json")]
        public async Task<GeneroMusical> GetGeneroMusical(int Id)
        {
            return await _IGeneneroMusicalApp.GetEntityByID(Id);
        }

        [Produces("application/json")]
        [HttpPost("/api/AdicionarGeneroMusical")]

        public async Task AdicionarGeneroMusical(string NomeGeneroMusical)
        {
            await _IGeneneroMusicalApp.Add(NomeGeneroMusical);
        }

        [Produces("application/json")]
        [HttpPut("/api/AtualizarGeneroMusical")]
        public async Task AtualizarGeneroMusical(int Id, string NovoGeneroMusical)
        {
            await _IGeneneroMusicalApp.Update(Id,NovoGeneroMusical);
        }
    }
}
