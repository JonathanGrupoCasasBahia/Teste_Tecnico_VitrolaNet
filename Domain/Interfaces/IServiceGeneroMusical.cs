using Entities.Entities;

namespace Domain.Interfaces
{
    public interface IServiceGeneroMusical
    {
        Task Add(string nome);

        Task Update(int id, string novoNome);

        Task<GeneroMusical> GetEntityByID(int Id);

        Task<List<GeneroMusical>> List();
    }
}
