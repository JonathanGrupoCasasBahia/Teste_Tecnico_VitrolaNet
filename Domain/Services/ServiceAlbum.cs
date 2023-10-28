using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Interfaces;

namespace Domain.Services
{
    public class ServiceAlbum : IServiceAlbum
    {
        private readonly IRepositoryAlbum _IRepositoryAlbum;
        private readonly IRepositoryArtista _IRepositoryArtista;

        public ServiceAlbum(IRepositoryAlbum IRepositoryAlbum, IRepositoryArtista IRepositoryArtista)
        {
            _IRepositoryAlbum = IRepositoryAlbum;
            _IRepositoryArtista = IRepositoryArtista;
        }

        public async Task Add(string NomeAlbum, int AnoLancamentoAlbum, int IdArtista)
        {
            if (string.IsNullOrWhiteSpace(NomeAlbum) || NomeAlbum.Length > 20)
            {
                throw new ArgumentException("Nome do album inválido.");
            }

            if (AnoLancamentoAlbum < 1 || AnoLancamentoAlbum.ToString().Length > 4)
            {
                throw new ArgumentException("Ano de lançamento deve conter 4 caracteres.");
            }

            var artistaExiste = await _IRepositoryArtista.GetEntityByID(IdArtista);

            if (artistaExiste == null)
            {
                throw new ArgumentException("O artista não existe.");
            }

            var novoAlbum = new Album 
            { Nome = NomeAlbum,
              AnoLancamento = AnoLancamentoAlbum,
              IdArtista = IdArtista
            };

            await _IRepositoryAlbum.Add(novoAlbum.Nome, novoAlbum.AnoLancamento, novoAlbum.IdArtista);
        }

        public async Task Delete(int Id)
        {
            var albumExiste = await _IRepositoryAlbum.GetEntityByID(Id);

            if (albumExiste == null)
            {
                throw new ArgumentException("O album não existe.");
            }
            if (albumExiste.Musicas.Count > 0)
            {
                throw new ArgumentException("Permitida a exclusão do album apenas se o mesmo estiver vazio.");
            }

            await _IRepositoryAlbum.Delete(Id);
        }

        public async Task<Album> GetEntityByID(int Id)
        {
            var albumExiste = await _IRepositoryAlbum.GetEntityByID(Id);

            if (albumExiste == null)
            {
                throw new ArgumentException("O album não existe.");
            }
            return await _IRepositoryAlbum.GetEntityByID(Id);
        }

        public async Task<List<Album>> GetEntityByName(string NomeAlbum)
        {
            var albumExiste = await _IRepositoryAlbum.GetEntityByName(NomeAlbum);

            if (albumExiste == null)
            {
                throw new ArgumentException("O album não existe.");
            }
            return await _IRepositoryAlbum.GetEntityByName(NomeAlbum);
        }

        public async Task<List<Album>> List()
        {
            return await _IRepositoryAlbum.List();
        }

        public async Task Update(int IdAlbum, string NovoNomeAlbum, int NovoAnoLancamentoAlbum, int NovoIdArtista)
        {
            if (string.IsNullOrWhiteSpace(NovoNomeAlbum) || NovoNomeAlbum.Length > 20)
            {
                throw new ArgumentException("Nome do album inválido.");
            }

            if (NovoAnoLancamentoAlbum < 1 || NovoAnoLancamentoAlbum.ToString().Length > 4)
            {
                throw new ArgumentException("Ano de lançamento deve conter 4 caracteres.");
            }

            var artistaExiste = await _IRepositoryArtista.GetEntityByID(NovoIdArtista);

            if (artistaExiste == null)
            {
                throw new ArgumentException("O artista não existe.");
            }

            var novoAlbum = new Album
            {
                Nome = NovoNomeAlbum,
                AnoLancamento = NovoAnoLancamentoAlbum,
                IdArtista = NovoIdArtista
            };

            await _IRepositoryAlbum.Update(IdAlbum, novoAlbum.Nome, novoAlbum.AnoLancamento, novoAlbum.IdArtista);
        }
    }
}
