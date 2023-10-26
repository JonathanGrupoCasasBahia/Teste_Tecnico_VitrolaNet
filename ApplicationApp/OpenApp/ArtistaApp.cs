using ApplicationApp.Interfaces;
using Domain.Interfaces;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.OpenApp
{
    public class ArtistaApp : IArtistaApp
    {
        private readonly IServiceArtista _IServiceArtista;

        public ArtistaApp(IServiceArtista IServiceArtista)
        {
            _IServiceArtista = IServiceArtista;
        }

        public async Task Add(string NomeArtista, int IdGenero)
        {
            await _IServiceArtista.Add(NomeArtista, IdGenero);
        }

        public async Task Delete(int Id)
        {
            await _IServiceArtista.Delete(Id);
        }

        public async Task<Artista> GetEntityByID(int Id)
        {
            return await _IServiceArtista.GetEntityByID(Id);
        }

        public async Task<List<Artista>> GetEntityByName(string Nome)
        {
            return await _IServiceArtista.GetEntityByName(Nome);
        }

        public async Task<List<Artista>> List()
        {
            return await _IServiceArtista.List();
        }

        public async Task Update(int Id, string NovoNomeArtista, int NovoIdGenero)
        {
            await _IServiceArtista.Update(Id, NovoNomeArtista, NovoIdGenero);
        }
    }
}
