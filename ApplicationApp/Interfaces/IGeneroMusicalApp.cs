using Entities.Entities;

namespace ApplicationApp.Interfaces
{
    public interface IGeneroMusicalApp
    {
        Task Add(string nome);

        Task Update(int id, string novoNome);

        Task<GeneroMusical> GetEntityByID(int Id);

        Task<List<GeneroMusical>> List();
    }
}
