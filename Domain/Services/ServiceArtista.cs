using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Interfaces;

namespace Domain.Services
{
    public class ServiceArtista : IServiceArtista
    {
        private readonly IRepositoryArtista _IRepositoryArtista;
        private readonly IRepositoryGeneroMusical _IRepositoryGeneroMusical;

        public ServiceArtista(IRepositoryArtista IRepositoryArtista, IRepositoryGeneroMusical IRepositoryGeneroMusical)
        {
            _IRepositoryArtista = IRepositoryArtista;
            _IRepositoryGeneroMusical = IRepositoryGeneroMusical;
        }

        public async Task Add(string NomeArtista, int IdGenero)
        {
            if (string.IsNullOrWhiteSpace(NomeArtista) || NomeArtista.Length > 50)
            {
                throw new ArgumentException("Nome do artista inválido.");
            }

            var generoExiste = await _IRepositoryGeneroMusical.GetEntityByID(IdGenero);

            if (generoExiste == null) 
            {
                throw new ArgumentException("O gênero musical não existe no catálogo.");
            }

            var novoArtista = new Artista{ Nome = NomeArtista };

            await _IRepositoryArtista.Add(novoArtista.Nome, IdGenero);
        }

        public async Task Delete(int Id)
        {
            var artistaExiste = await _IRepositoryArtista.GetEntityByID(Id);

            if (artistaExiste == null)
            {
                throw new ArgumentException("O artista não existe.");
            }

            await _IRepositoryArtista.Delete(Id);
        }

        public async Task<Artista> GetEntityByID(int Id)
        {
            if (_IRepositoryArtista.GetEntityByID == null)
            {
                throw new ArgumentException("Id não encontrado.");
            }
                return await _IRepositoryArtista.GetEntityByID(Id);
        }

        public async Task<List<Artista>> GetEntityByName(string TrechoNome)
        {
            if(_IRepositoryArtista.GetEntityByName == null)
            {
                throw new ArgumentException ("Artista não existe.");
            }
            return await _IRepositoryArtista.GetEntityByName(TrechoNome);
        }

        public async Task<List<Artista>> List()
        {
            return await _IRepositoryArtista.List();
        }

        public async Task Update(int Id, string NovoNomeArtista, int NovoIdGenero)
        {
            var artistaExiste = await _IRepositoryArtista.GetEntityByID(Id);

            if (artistaExiste == null)
            {
                throw new InvalidOperationException("O artista não existe.");
            }

            var generoExiste = await _IRepositoryGeneroMusical.GetEntityByID(NovoIdGenero);

            if (generoExiste == null)
            {
                throw new ArgumentException("O gênero musical não existe no catálogo.");
            }

            await _IRepositoryArtista.Update(Id, NovoNomeArtista, NovoIdGenero);
        }
    }
}
