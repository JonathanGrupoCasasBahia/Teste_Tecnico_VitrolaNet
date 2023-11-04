using Domain.Interfaces;
using Domain.Services;
using Entities.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace TestMock.Services
{
    public class ServiceAlbumTest
    {
        private readonly IServiceAlbum _IServiceAlbum;
        private readonly Mock<IRepositoryAlbum> repositoryAlbum;
        private readonly Mock<IRepositoryArtista> repositoryArtista;

        public ServiceAlbumTest()
        {
            repositoryAlbum = new Mock<IRepositoryAlbum>();
            repositoryArtista = new Mock<IRepositoryArtista>();

            _IServiceAlbum = new ServiceAlbum(repositoryAlbum.Object, repositoryArtista.Object);
        }

        [Fact(DisplayName = "Nao criar album com mais de 20 caracteres")]
        public async Task AddAlbumInvalido()
        {
            string nomeAlbum = "Album Com Mais de Vinte Caracteres";
            int anoLancamento = 2023;
            int idArtista = 1;

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Add(nomeAlbum,anoLancamento,idArtista));

            Assert.Equal("Nome do album inválido.", exception.Message);
        }

        [Fact(DisplayName = "Nao criar album com ano de lancamento com menos ou mais de 4 caracteres")]
        public async Task AddAlbumAnoInvalido()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 202;
            int idArtista = 1;

            Album album = new Album();

            repositoryArtista.Setup(repository => repository.GetEntityByID(idArtista)).ReturnsAsync(new Artista { Id = idArtista });
            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(album);
            


            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Add(nomeAlbum,anoLancamento,idArtista));

            Assert.Equal("Ano de lançamento deve conter 4 caracteres.", exception.Message);
        }

        [Fact(DisplayName ="Nao criar album, artista nao existe")]
        public async Task AddAlbumArtistaNaoExiste()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            Artista artista = null;

            repositoryArtista.Setup(repository => repository.GetEntityByID(idArtista)).ReturnsAsync(artista);
            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Album { IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista });

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Add(nomeAlbum, anoLancamento, idArtista));

            Assert.Equal("O artista não existe.", exception.Message);
        }
                
        [Fact(DisplayName = "criar Album")]
        public async void AddAlbum()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            repositoryArtista.Setup(repository => repository.GetEntityByID(idArtista)).ReturnsAsync(new Artista { Id = idArtista });
            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Album { IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista });

            await _IServiceAlbum.Add(nomeAlbum,anoLancamento,idArtista);

            repositoryAlbum.Verify(repository => repository.Add(nomeAlbum,anoLancamento,idArtista), Times.Once);
        }

        //--------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao atualizar album com mais de 20 caracteres")]
        public async Task UpdateAlbumInvalido()
        {
            int id = 1; 
            string nomeAlbum = "Album Com Mais de Vinte Caracteres";
            int anoLancamento = 2023;
            int idArtista = 1;

            Album album = new Album()
            {
                IdAlbum = id,
                Nome = nomeAlbum,
                AnoLancamento = anoLancamento,
                IdArtista = idArtista
            };

            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(album);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Update(id, nomeAlbum, anoLancamento, idArtista));

            Assert.Equal("Nome do album inválido.", exception.Message);
        }

        [Fact(DisplayName ="Nao atualizar album se ele nao existe")]
        public async Task UpdateAlbumNaoExiste()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 202;
            int idArtista = 1;

            Album album = null;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(album);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(()=> _IServiceAlbum.Update(id,nomeAlbum, anoLancamento,idArtista));

            Assert.Equal("O album não existe.", exception.Message);
        }

        [Fact(DisplayName = "Nao atualizar album com ano de lancamento com menos ou mais de 4 caracteres")]
        public async Task UpdateAlbumAnoInvalido()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 202;
            int idArtista = 1;

            Album album = new Album();

            repositoryArtista.Setup(repository => repository.GetEntityByID(idArtista)).ReturnsAsync(new Artista { Id = idArtista });
            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(album);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Update(id, nomeAlbum, anoLancamento, idArtista));

            Assert.Equal("Ano de lançamento deve conter 4 caracteres.", exception.Message);
        }

        [Fact(DisplayName = "Nao atualizar album, artista nao existe")]
        public async Task UpdateAlbumArtistaNaoExiste()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            Artista artista = null;

            repositoryArtista.Setup(repository => repository.GetEntityByID(idArtista)).ReturnsAsync(artista);
            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Album { IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista });

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Update(id, nomeAlbum, anoLancamento, idArtista));

            Assert.Equal("O artista não existe.", exception.Message);
        }

        [Fact(DisplayName = "Atualizar Album")]
        public async void UpdateAlbum()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            repositoryArtista.Setup(repository => repository.GetEntityByID(idArtista)).ReturnsAsync(new Artista { Id = idArtista });
            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Album { IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista });

            await _IServiceAlbum.Update(id, nomeAlbum, anoLancamento, idArtista);

            repositoryAlbum.Verify(repository => repository.Update(id, nomeAlbum, anoLancamento, idArtista), Times.Once);
        }

        //---------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao deletar album, album nao existe")]
        public async void DeleteAlbumNaoExiste() 
        {
            int id = 1;

            Album album = null;

            repositoryAlbum.Setup(reposytory => reposytory.GetEntityByID(id)).ReturnsAsync((album));

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Delete(id));

            Assert.Equal("O album não existe.", exception.Message);
        }

        [Fact(DisplayName = "Nao deletar album, album possui musicas")]
        public async void DeleteAlbumComMusicas()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;

            List<Musica> musicas = new List<Musica>()
            {
                new Musica{Nome = "musica", Id = 1, IdAlbum = 2, Ordem = 1}
            };

            repositoryAlbum.Setup(reposytory => reposytory.GetEntityByID(id)).ReturnsAsync(new Album {IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = id, Musicas = musicas });

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.Delete(id));

            Assert.Equal("Para excluir o album, remova todas as músicas.", exception.Message);
        }

        [Fact(DisplayName = "Nao deletar album, album possui musicas")]
        public async void DeleteAlbumSuccess()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;

            List<Musica> musicas = new List<Musica>();

            repositoryAlbum.Setup(reposytory => reposytory.GetEntityByID(id)).ReturnsAsync(new Album { IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = id, Musicas = musicas});

            await _IServiceAlbum.Delete(id);

            repositoryAlbum.Verify(repository => repository.Delete(id), Times.Once);
        }

        //---------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao achar album, nome null")]
        public async Task GetByNameNull()
        {
            string nome = "nome album valido";

            List<Album> albums = new List<Album>();

            repositoryAlbum.Setup(repository => repository.GetEntityByName(nome)).ReturnsAsync((albums));

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.GetEntityByName(nome));

            Assert.Equal("O album não existe.", exception.Message);
        }        

        [Fact(DisplayName = "Encontrar o album pelo Nome")]
        public async Task GetByNameSuccess()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            List<Album> albuns = new List<Album>()
            {
                new Album {IdAlbum = id, Nome =  nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista}
            };

            repositoryAlbum.Setup(repository => repository.GetEntityByName(nomeAlbum)).ReturnsAsync(albuns);

            List <Album> albumRetornado = await _IServiceAlbum.GetEntityByName(nomeAlbum);

            Assert.Equal(albuns, albumRetornado);
        }

        [Fact(DisplayName = "Nao achar album, id null")]
        public async Task GetByIdNull()
        {
            int id = 20;
            Album album = null;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(album);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.GetEntityByID(id));

            Assert.Equal("O album não existe.", exception.Message);
        }

        [Fact(DisplayName = "Encontrar o album pelo Id")]
        public async Task GetByIdSuccess()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Album { IdAlbum = id, Nome = nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista });

            Album albumRetornado = await _IServiceAlbum.GetEntityByID(id);

            Assert.Equal(id, albumRetornado.IdAlbum);

            Assert.Equal(nomeAlbum, albumRetornado.Nome);

            Assert.Equal(anoLancamento, albumRetornado.AnoLancamento);

            Assert.Equal(idArtista, albumRetornado.IdArtista);
        }

        [Fact(DisplayName = "Listar albuns (lista vazia)")]
        public async Task ListarAlbunsEmptyList()
        {
            List<Album> albums = new List<Album>();

            repositoryAlbum.Setup(repository => repository.List()).ReturnsAsync(albums);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceAlbum.List());

            Assert.Equal("Lista de albuns vazia", exception.Message);
        }

        [Fact(DisplayName = "Listar albuns com sucesso")]
        public async Task ListAlbunsSuccess()
        {
            int id = 1;
            string nomeAlbum = "Nome album valido";
            int anoLancamento = 2023;
            int idArtista = 1;

            List<Album> albuns = new List<Album>
            {
                new Album {IdAlbum = id, Nome =  nomeAlbum, AnoLancamento = anoLancamento, IdArtista = idArtista},
                new Album {IdAlbum = 2, Nome =  "album dois", AnoLancamento = 2023, IdArtista = 7}
            };

            repositoryAlbum.Setup(repository => repository.List()).ReturnsAsync(albuns);

            List<Album> albunsRetornados = await _IServiceAlbum.List();

            Assert.Equal(albuns, albunsRetornados);
        }
    }
}
