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
            var generoExiste = await _IRepositoryGeneroMusical.GetEntityByName(NomeGeneroMusical);

            if (string.IsNullOrWhiteSpace(NomeGeneroMusical) || NomeGeneroMusical.Length >20)
            {
                throw new ArgumentException("Nome do gênero musical inválido.");
            }            
            else if (generoExiste != null)
            {
                throw new ArgumentException("O gênero musical já existe no catálogo.");
            }

            var novoGenero = new GeneroMusical { Nome = NomeGeneroMusical };

            await _IRepositoryGeneroMusical.Add(novoGenero.Nome);
        }

        public async Task<GeneroMusical> GetEntityByID(int Id)
        {
            var generoExiste = await _IRepositoryGeneroMusical.GetEntityByID(Id);

            if (generoExiste == null)
            {
                throw new ArgumentException("O gênero musical não existe no catálogo.");
            }

            return await _IRepositoryGeneroMusical.GetEntityByID(Id);
        }

        public async Task<List<GeneroMusical>> List()
        {
            var verificaLista = await _IRepositoryGeneroMusical.List();

            if(verificaLista.Count() == 0)
            {
                throw new ArgumentException("Lista de generos vazia");
            }

            return await _IRepositoryGeneroMusical.List();
        }

        public async Task Update(int Id, string NovoNomeGeneroMusical)
        {
            var generoExiste = await _IRepositoryGeneroMusical.GetEntityByID(Id);
            var novoNomeGerenoExiste = await _IRepositoryGeneroMusical.GetEntityByName(NovoNomeGeneroMusical);

            if (string.IsNullOrWhiteSpace(NovoNomeGeneroMusical) || NovoNomeGeneroMusical.Length > 20)
            {
                throw new ArgumentException("Nome do gênero musical inválido.");
            }            
            else if (generoExiste == null)
            {
                throw new ArgumentException("O gênero musical não existe no catálogo.");
            }
            else if (novoNomeGerenoExiste != null)
            {
                throw new ArgumentException("O novo nome do gênero musical já existe no catálogo.");
            }

            generoExiste.Nome = NovoNomeGeneroMusical;

            await _IRepositoryGeneroMusical.Update(Id, generoExiste.Nome);
        }
    }
}
