using Entities.Entities;

namespace Domain.Interfaces
{
    public interface IServiceAlbum
    {
        Task Add(string NomeAlbum, int AnoLancamentoAlbum, int IdArtista);

        Task Update(int IdAlbum, string NovoNomeAlbum, int NovoAnoLancamentoAlbum, int NovoIdArtista);

        Task Delete(int Id);

        Task<Album> GetEntityByID(int Id);

        Task<List<Album>> GetEntityByName(string NomeAlbum);

        Task<List<Album>> List();
    }
}
