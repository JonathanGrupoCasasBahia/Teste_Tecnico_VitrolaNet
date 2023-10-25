﻿using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Interfaces;

namespace Domain.Services
{
    public class ServiceGeneroMusical : IServiceGeneroMusical
    {
        private readonly IRepositoryGeneroMusical _IRepositoryGeneroMusical;

        public ServiceGeneroMusical(IRepositoryGeneroMusical IRepositoryGeneroMusical)
        {
            _IRepositoryGeneroMusical = IRepositoryGeneroMusical;
        }
        
        public async Task Add(string NomeGeneroMusical)
        {
            if (string.IsNullOrWhiteSpace(NomeGeneroMusical) || NomeGeneroMusical.Length >20)
            {
                throw new ArgumentException("Nome do gênero musical inválido.");
            }
            var generoExiste = _IRepositoryGeneroMusical.GetEntityByName(NomeGeneroMusical);
            if (generoExiste != null)
            {
                throw new InvalidOperationException("O gênero musical já existe no catálogo.");
            }

            var novoGenero = new GeneroMusical { Nome = NomeGeneroMusical };
            await _IRepositoryGeneroMusical.Add(novoGenero);
        }

        public Task<GeneroMusical> GetEntityByID(int Id)
        {
            return _IRepositoryGeneroMusical.GetEntityByID(Id);
        }

        public async Task<List<GeneroMusical>> List()
        {
            return await _IRepositoryGeneroMusical.List();
        }

        public async Task Update(int Id, string NovoNomeGeneroMusical)
        {
            if (string.IsNullOrWhiteSpace(NovoNomeGeneroMusical) || NovoNomeGeneroMusical.Length > 20)
            {
                throw new ArgumentException("Nome do gênero musical inválido.");
            }
            var generoExiste = _IRepositoryGeneroMusical.GetEntityByName(NovoNomeGeneroMusical);
            if (generoExiste != null)
            {
                throw new InvalidOperationException("O gênero musical já existe no catálogo.");
            }

            var novoGenero = new GeneroMusical { Nome = NovoNomeGeneroMusical };
            await _IRepositoryGeneroMusical.Update(novoGenero);
        }
    }
}
