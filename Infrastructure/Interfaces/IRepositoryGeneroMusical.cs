using Entities.Entities;

namespace Infrastructure.Interfaces
{
    public interface IRepositoryGeneroMusical
    {
        Task Add(GeneroMusical GeneroMusical);

        Task Update(GeneroMusical GeneroMusical);

        Task<GeneroMusical> GetEntityByID(int Id);

        Task<GeneroMusical?> GetEntityByName(string Name);

        Task<List<GeneroMusical>> List();
    }
}
