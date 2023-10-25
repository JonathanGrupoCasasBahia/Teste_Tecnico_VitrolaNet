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
        Task Add(string NomeGeneroMusical);

        Task Update(int Id, string NovoNomeGeneroMusical);

        Task<GeneroMusical> GetEntityByID(int Id);

        Task<List<GeneroMusical>> List();
    }
}
