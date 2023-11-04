using Entities.Entities;

namespace Infrastructure.Interfaces
{
    public interface IRepositoryArtista
    {
        Task Add(string NomeArtista, int IdGenero);

        Task Update(int Id, string NovoNomeArtista, int NovoIdGenero);

        Task Delete(int Id);

        Task<Artista> GetEntityByID(int Id);

        Task<List<Artista>> GetEntityByName(string TrechoNome);

        Task<List<Artista>> List();
    }
}
