using Entities.Entities;

namespace Domain.Interfaces
{
    public interface IServiceArtista
    {
        Task Add(string NomeArtista, int IdGenero);

        Task Update(int Id, string NovoNomeArtista, int NovoIdGenero);

        Task Delete(int Id);

        Task<Artista> GetEntityByID(int Id);

        Task<List<Artista>> GetEntityByName(string Nome);

        Task<List<Artista>> List();
    }
}
