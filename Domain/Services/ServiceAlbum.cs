using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Interfaces;

namespace Domain.Services
{
    //TODO Dica: No nome das classe fazemos o inverso, nesse caso seria AlbumService
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
            var artistaExiste = await _IRepositoryArtista.GetEntityByID(IdArtista);

            if (string.IsNullOrWhiteSpace(NomeAlbum) || NomeAlbum.Length > 20)
            {
                throw new ArgumentException("Nome do album inválido.");
            }
            else if (AnoLancamentoAlbum.ToString().Length != 4)
            {
                throw new ArgumentException("Ano de lançamento deve conter 4 caracteres.");
            }
            else if (artistaExiste == null)
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
            else if (albumExiste.Musicas.Count > 0)
            {
                throw new ArgumentException("Para excluir o album, remova todas as músicas.");
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

            if (albumExiste.Count == 0)
            {
                //TODO Usando a exceção ele retorna o stack trace para usuário, e não devemos exibir isso para terceiros. O ideal é criar um objeto que consiga recebe a validação e dar uma resposta amigável.
                throw new ArgumentException("O album não existe.");
            }

            //TODO Invés de chamar o banco novamente poderia usar o retorna da variável albumExiste. Sempre que possível fazer o minimo de requisição ao BD.
            return await _IRepositoryAlbum.GetEntityByName(NomeAlbum);
        }

        public async Task<List<Album>> List()
        {
            var verificaAlbuns = await _IRepositoryAlbum.List();

            if(verificaAlbuns.Count == 0)
            {
                throw new ArgumentException("Lista de albuns vazia");
            }
            return await _IRepositoryAlbum.List();
        }

        public async Task Update(int IdAlbum, string NovoNomeAlbum, int NovoAnoLancamentoAlbum, int NovoIdArtista)
        {
            var artistaExiste = await _IRepositoryArtista.GetEntityByID(NovoIdArtista);
            var albumExiste = await _IRepositoryAlbum.GetEntityByID(IdAlbum);

            if (albumExiste == null)
            {
                throw new ArgumentException("O album não existe.");
            }
            else if (string.IsNullOrWhiteSpace(NovoNomeAlbum) || NovoNomeAlbum.Length > 20)
            {
                throw new ArgumentException("Nome do album inválido.");
            }
            else if (NovoAnoLancamentoAlbum.ToString().Length != 4)
            {
                throw new ArgumentException("Ano de lançamento deve conter 4 caracteres.");
            }
            else if (artistaExiste == null)
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
