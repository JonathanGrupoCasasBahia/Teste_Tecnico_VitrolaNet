using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
