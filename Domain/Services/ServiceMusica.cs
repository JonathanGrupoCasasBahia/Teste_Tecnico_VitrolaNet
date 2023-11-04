using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Interfaces;

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

            if (string.IsNullOrWhiteSpace(NomeMusica) || NomeMusica.Length > 30)
            {
                throw new ArgumentException("Nome da música inválido.");
            }
            else if(albumExiste == null)
            {
                throw new ArgumentException("Album informado não existe.");
            }

            await _IRepositoryMusica.Add(NomeMusica, Ordem, IdAlbum);

        }

        public async Task Delete(int IdMusica, int IdAlbum)
        {
            var musicaEAlbumExiste = await _IRepositoryMusica.GetEntityByIDMusicaIdAlbum(IdMusica,IdAlbum);
            var MusicasNoAlbum = await _IRepositoryAlbum.GetEntityByID(IdAlbum);

            if (musicaEAlbumExiste == null)
            {
                throw new ArgumentException("Musica não pertence ao album.");
            }            
            else if(MusicasNoAlbum.Musicas.Count == 1) 
            {
                await _IRepositoryMusica.Delete(IdMusica,IdAlbum);
                await _IRepositoryAlbum.Delete(IdAlbum);
            }
            else if(MusicasNoAlbum.Musicas.Count > 1)
            {
                await _IRepositoryMusica.Delete(IdMusica, IdAlbum);
            }
        }

        public async Task<Musica> GetEntityByID(int Id)
        {
            var musicaExiste = await _IRepositoryMusica.GetEntityByID(Id);

            if (musicaExiste == null)
            {
                throw new ArgumentException("Musica não existe.");
            }

            return await _IRepositoryMusica.GetEntityByID(Id);
        }

        public async Task<List<Musica>> GetEntityByName(string TrechoNomeMusica)
        {
            var musicaExiste = await _IRepositoryMusica.GetEntityByName(TrechoNomeMusica);

            if (musicaExiste.Count == 0)
            {
                throw new ArgumentException("Musica não existe.");
            }

            return await _IRepositoryMusica.GetEntityByName(TrechoNomeMusica);
        }

        public async Task<List<Musica>> List()
        {
            var verificaLista = await _IRepositoryMusica.List();

            if(verificaLista.Count == 0)
            {
                throw new ArgumentException("Lista de musicas vazia");
            }

            return await _IRepositoryMusica.List();
        }

        public async Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum)
        {
            var musicaExiste = await _IRepositoryMusica.GetEntityByID(Id);
            var novoAlbumExiste = await _IRepositoryAlbum.GetEntityByID(NovoIdAlbum);

            if(string.IsNullOrWhiteSpace(NovoNomeMusica) || NovoNomeMusica.Length > 30)
            {
                throw new ArgumentException("Nome da nova música inválido.");
            }
            else if (musicaExiste == null)
            {
                throw new ArgumentException("Musica não existe.");
            }
            else if (novoAlbumExiste == null)
            {
                throw new ArgumentException("Album não existe.");
            }

            await _IRepositoryMusica.Update(Id, NovoNomeMusica, NovaOrdem, NovoIdAlbum);
        }
    }
}
