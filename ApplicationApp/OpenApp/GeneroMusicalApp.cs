using ApplicationApp.Interfaces;
using Domain.Interfaces;
using Entities.Entities;

namespace ApplicationApp.OpenApp
{
    public class GeneroMusicalApp : IGeneroMusicalApp
    {
        private readonly IServiceGeneroMusical _IServiceGeneroMusical;

        public GeneroMusicalApp (IServiceGeneroMusical IServiceGeneroMusical)
        {
            _IServiceGeneroMusical = IServiceGeneroMusical;
        }

        public async Task Add(string nome)
        {
            await _IServiceGeneroMusical.Add(nome);
        }

        public async Task<GeneroMusical> GetEntityByID(int Id)
        {
            return await _IServiceGeneroMusical.GetEntityByID(Id);
        }

        public async Task<List<GeneroMusical>> List()
        {
            return await _IServiceGeneroMusical.List();
        }

        public async Task Update(int id, string novoNome)
        {
            await _IServiceGeneroMusical.Update(id, novoNome);
        }
    }
}
