using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.Interfaces
{
    public interface IArtistaApp
    {
        Task Add(string NomeArtista, int IdGenero);

        Task Update(int Id, string NovoNomeArtista, int NovoIdGenero);

        Task Delete(int Id);

        Task<Artista> GetEntityByID(int Id);

        Task<List<Artista>> GetEntityByName(string Nome);

        Task<List<Artista>> List();
    }
}
