using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.Interfaces
{
    public interface IAlbumApp
    {
        Task Add(string NomeAlbum, int AnoLancamentoAlbum, int IdArtista);

        Task Update(string NovoNomeAlbum, int NovoAnoLancamentoAlbum, int NovoIdArtista);

        Task Delete(int Id);

        Task<Album> GetEntityByID(int Id);

        Task<List<Album>> GetEntityByName(string NomeAlbum);

        Task<List<Album>> List();
    }
}
