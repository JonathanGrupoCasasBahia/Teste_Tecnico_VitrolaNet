using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.Interfaces
{
    public interface IGeneroMusicalApp
    {
        Task Add(string NomeGeneroMusical);

        Task Update(int Id, string NovoNomeGeneroMusical);

        Task<GeneroMusical> GetEntityByID(int Id);

        Task<List<GeneroMusical>> List();
    }
}
