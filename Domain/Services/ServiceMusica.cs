using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Interfaces;
using System.Runtime.Intrinsics.Arm;

namespace Domain.Services
{
    public class ServiceMusica : IServiceMusica
    {
        private readonly IRepositoryMusica _IRepositoryMusica;
        private readonly IRepositoryAlbum _IRepositoryAlbum;

        public ServiceMusica(IRepositoryMusica IRepositoryMusica, IRepositoryAlbum IRepositoryAlbum)
        {
            _IRepositoryMusica = IRepositoryMusica;
            _IRepositoryAlbum = IRepositoryAlbum;
        }

        public async Task Add(string NomeMusica, int Ordem, int IdAlbum)
        {
            var albumExiste = await _IRepositoryAlbum.GetEntityByID(IdAlbum);

            var ordemExiste = await _IRepositoryMusica.GetByOrdemIdAlbum(Ordem, IdAlbum);

            if (string.IsNullOrWhiteSpace(NomeMusica) || NomeMusica.Length > 30)
            {
                throw new ArgumentException("Nome da música inválido.");
            }
            else if (albumExiste == null)
            {
                throw new ArgumentException("Album informado não existe.");
            }
            else if(ordemExiste != null)
            {
                throw new ArgumentException("Ordem informada já existe no album.");
            }

            await _IRepositoryMusica.Add(NomeMusica, Ordem, IdAlbum);

        }

        public async Task Delete(int Id, int IdAlbum)
        {
            var albumExiste = await _IRepositoryAlbum.GetEntityByID(IdAlbum);
            var musicaExiste = await _IRepositoryMusica.GetEntityByID(Id);

            if (albumExiste == null)
            {
                throw new ArgumentException("Album não existe.");
            }
            else if(musicaExiste == null)
            {
                throw new ArgumentException("Musica não existe");
            }
            
            if(albumExiste.Musicas.Count == 1) 
            {
                await _IRepositoryMusica.Delete(Id);
                await _IRepositoryAlbum.Delete(IdAlbum);
                
            }
            if(albumExiste.Musicas.Count > 1)
            {
                await _IRepositoryMusica.Delete(Id);
            }
        }

        public async Task<Musica> GetEntityByID(int Id)
        {
            var musicaExiste = await _IRepositoryMusica.GetEntityByID(Id);

            if (musicaExiste == null)
            {
                throw new ArgumentException("Musica não existe");
            }

            return await _IRepositoryMusica.GetEntityByID(Id);
        }

        public async Task<List<Musica>> GetEntityByName(string TrechoNomeMusica)
        {
            var musicaExiste = await _IRepositoryMusica.GetEntityByName(TrechoNomeMusica);

            if (musicaExiste == null)
            {
                throw new ArgumentException("Musica não existe.");
            }

            return await _IRepositoryMusica.GetEntityByName(TrechoNomeMusica);
        }

        public async Task<List<Musica>> List()
        {
            return await _IRepositoryMusica.List();
        }

        public async Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum)
        {
            var musicaExiste = await _IRepositoryMusica.GetEntityByID(Id);

            var ordemExiste = await _IRepositoryMusica.GetByOrdemIdAlbum(NovaOrdem, NovoIdAlbum);

            var novoAlbumExiste = await _IRepositoryAlbum.GetEntityByID(NovoIdAlbum);

            if (musicaExiste == null)
            {
                throw new ArgumentException("Musica não existe.");
            }
            else if (novoAlbumExiste == null)
            {
                throw new ArgumentException("Album não existe.");
            }
            else if (ordemExiste != null)
            {
                throw new ArgumentException("Ordem informada já existe no album.");
            }

            await _IRepositoryMusica.Update(Id, NovoNomeMusica, NovaOrdem, NovoIdAlbum);
        }
    }
}
