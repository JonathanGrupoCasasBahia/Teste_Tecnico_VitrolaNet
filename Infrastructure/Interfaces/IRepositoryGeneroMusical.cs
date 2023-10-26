using Entities.Entities;

namespace Infrastructure.Interfaces
{
    public interface IRepositoryGeneroMusical
    {
        Task Add(string nome);

        Task Update(int Id, string novoNome);

        Task<GeneroMusical> GetEntityByID(int Id);

        Task<GeneroMusical> GetEntityByName(string Name);

        Task<List<GeneroMusical>> List();
    }
}
