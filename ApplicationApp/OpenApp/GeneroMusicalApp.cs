using ApplicationApp.Interfaces;
using Domain.Interfaces;
using Domain.Services;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.OpenApp
{
    public class GeneroMusicalApp : IGeneroMusicalApp
    {
        private readonly IServiceGeneroMusical _IServiceGeneroMusical;

        public GeneroMusicalApp (IServiceGeneroMusical IServiceGeneroMusical)
        {
            _IServiceGeneroMusical = IServiceGeneroMusical;
        }

        public async Task Add(string NomeGeneroMusical)
        {
            await _IServiceGeneroMusical.Add(NomeGeneroMusical);
        }

        public async Task<GeneroMusical> GetEntityByID(int Id)
        {
            return await _IServiceGeneroMusical.GetEntityByID(Id);
        }

        public async Task<List<GeneroMusical>> List()
        {
            return await _IServiceGeneroMusical.List();
        }

        public async Task Update(int Id, string NovoNomeGeneroMusical)
        {
            await _IServiceGeneroMusical.Update(Id, NovoNomeGeneroMusical);
        }
    }
}
