using Entities.Entities;

namespace Infrastructure.Interfaces
{
    public  interface IRepositoryAlbum
    {
        Task Add(string NomeAlbum, int AnoLancamento, int IdArtista);

        Task Update(int IdAlbum, string NomeAlbum, int AnoLancamento, int IdArtista);

        Task Delete(int Id);

        Task<Album> GetEntityByID(int Id);

        Task<List<Album>> GetEntityByName(string NomeAlbum);

        Task<List<Album>> List();
    }
}
