using Domain.Interfaces;
using Domain.Services;
using Entities.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repository;
using Moq;

namespace TestMock.Services
{
    public  class ServiceArtistaTest
    {

        private readonly IServiceArtista _IServiceArtista;
        private readonly Mock<IRepositoryArtista> repositoryArtista;
        private readonly Mock<IRepositoryGeneroMusical> repositoryGeneroMusical;

        public ServiceArtistaTest()
        {
            repositoryArtista = new Mock<IRepositoryArtista>();
            repositoryGeneroMusical = new Mock<IRepositoryGeneroMusical>();

            _IServiceArtista = new ServiceArtista(repositoryArtista.Object, repositoryGeneroMusical.Object);
        }

        [Fact(DisplayName = "Nao criar Artista com mais de 50 caracteres")]
        public async Task AddArtistaInvalido()
        {
            string nomeArtista = "Artista Com Mais de Vinte Caracteres nao eh permitido";
            int idGenero= 2;

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.Add(nomeArtista, idGenero));

            Assert.Equal("Nome do artista inválido.", exception.Message);
        }
        
        [Fact(DisplayName = "Nao criar artista, genero nao existe")]
        public async Task AddArtistaAlbumNaoExiste()
        {
            int id = 1;
            string nomeArtista = "Nome musica valido";
            int idGenero= 1;

            GeneroMusical genero = null;

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(idGenero)).ReturnsAsync(genero);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.Add(nomeArtista, idGenero));

            Assert.Equal("O gênero musical não existe no catálogo.", exception.Message);
        }
        
        [Fact(DisplayName = "criar Artista")]
        public async void AddArtista()
        {
            int id = 1;
            string nomeArtista = "Nome musica valido";
            int idGenero= 1;

            Artista artista = null;
            GeneroMusical generoMusical = new GeneroMusical();

            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(artista);
            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(idGenero)).ReturnsAsync(generoMusical);

            await _IServiceArtista.Add(nomeArtista, idGenero);

            repositoryArtista.Verify(repository => repository.Add(nomeArtista, idGenero), Times.Once);
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [Fact(DisplayName = "Nao atualizar artista com mais de 50 caracteres")]
        public async Task UpdateArtistaInvalido()
        {
            int id = 1;
            string nomeArtista = "Artista Com Mais de Vinte Caracteres nao eh permitido";
            int idGenero= 1;

            GeneroMusical genero = new GeneroMusical()
            {
                Id = idGenero,
                Nome = "genero"
            };

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(idGenero)).ReturnsAsync(genero);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.Update(id, nomeArtista, idGenero));


            Assert.Equal("Nome do artista inválido.", exception.Message);
        }
        
        [Fact(DisplayName = "Nao atualizar artista, ele nao existe")]
        public async Task UpdateArtistaNaoExiste()
        {
            int id = 1;
            string nomeArtista = "Nome artista valido";
            int idGenero= 1;

            Artista artista = null;

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(idGenero)).ReturnsAsync(new GeneroMusical{ Id = idGenero});
            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(artista);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.Update(id, nomeArtista, idGenero));

            Assert.Equal("O artista não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Nao atualizar artista, album nao existe")]
        public async Task UpdateArtistaAlbumNaoExiste()
        {
            int id = 1;
            string nomeArtista = "Nome artista valido";
            int idGenero= 1;

            GeneroMusical genero = null;

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(idGenero)).ReturnsAsync(genero);
            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Artista { Id = id, Nome = nomeArtista, IdGenero= idGenero});

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.Update(id, nomeArtista, idGenero));

            Assert.Equal("O gênero musical não existe no catálogo.", exception.Message);
        }
        
        [Fact(DisplayName = "Atualizar Artista")]
        public async void UpdateArtista()
        {
            int id = 1;
            string nomeArtista = "Nome artista valido";
            int idGenero= 1;

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(idGenero)).ReturnsAsync(new GeneroMusical{ Id= idGenero});
            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Artista { Id = id, Nome = nomeArtista, IdGenero= idGenero});

            await _IServiceArtista.Update(id, nomeArtista, idGenero);

            repositoryArtista.Verify(repository => repository.Update(id, nomeArtista, idGenero), Times.Once);
        }

        //---------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao deletar artista, artista nao existe")]
        public async void DeleteAlbumNaoExiste()
        {
            int id = 1;
            Artista artista = null;

            repositoryArtista.Setup(reposytory => reposytory.GetEntityByID(id)).ReturnsAsync((artista));

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.Delete(id));

            Assert.Equal("O artista não existe.", exception.Message);
        }

        [Fact(DisplayName ="Deletar Artista")]
        public async Task DeleteArtista()
        {
            int id = 1;
            string nome = "nome valido";
            int idGenero = 2;

            Artista artista = new Artista
            {
                Id = id,
                Nome = nome,
                IdGenero = idGenero
            };

            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync((artista));

            await _IServiceArtista.Delete(id);

            repositoryArtista.Verify(repository => repository.Delete(id), Times.Once);

        }
        //---------------------------------------------------------------------------------------------------------------
        
        [Fact(DisplayName = "Nao achar artista, nome null")]
        public async Task GetByNameNull()
        {
            string nome = "nome artista valido";

            List<Artista> artistas = new List<Artista>();

            repositoryArtista.Setup(repository => repository.GetEntityByName(nome)).ReturnsAsync((artistas));

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.GetEntityByName(nome));

            Assert.Equal("Artista não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Encontrar o artista pelo Nome")]
        public async Task GetByNameSuccess()
        {
            int id = 1;
            string nomeArtista = "Nome artista valido";
            int idGenero= 1;

            List<Artista> artistas = new List<Artista>()
        {
            new Artista {Id = id, Nome =  nomeArtista, IdGenero= idGenero}
        };

            repositoryArtista.Setup(repository => repository.GetEntityByName(nomeArtista)).ReturnsAsync(artistas);

            List<Artista> artistaRetornado = await _IServiceArtista.GetEntityByName(nomeArtista);

            Assert.Equal(artistas, artistaRetornado);
        }
        
        [Fact(DisplayName = "Nao achar artista, id null")]
        public async Task GetByIdNull()
        {
            int id = 20;
            Artista artista = null;

            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(artista);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.GetEntityByID(id));

            Assert.Equal("Artista não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Encontrar o artista pelo Id")]
        public async Task GetByIdSuccess()
        {
            int id = 1;
            string nomeArtista = "Nome artista valido";
            int idGenero= 1;

            repositoryArtista.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Artista { Id = id, Nome = nomeArtista, IdGenero= idGenero});

            Artista artistaRetornado = await _IServiceArtista.GetEntityByID(id);

            Assert.Equal(id, artistaRetornado.Id);

            Assert.Equal(nomeArtista, artistaRetornado.Nome);

            Assert.Equal(idGenero, artistaRetornado.IdGenero);
        }
        
        [Fact(DisplayName = "Listar artistas (lista vazia)")]
        public async Task ListarAlbunsEmptyList()
        {
            List<Artista> artistas = new List<Artista>();

            repositoryArtista.Setup(repository => repository.List()).ReturnsAsync(artistas);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceArtista.List());

            Assert.Equal("Lista de artistas vazia.", exception.Message);
        }
        
        [Fact(DisplayName = "Listar artistas com sucesso")]
        public async Task ListAlbunsSuccess()
        {
            int id = 1;
            string nomeArtista = "Nome artista valido";
            int idGenero= 1;

            List<Artista> artistas = new List<Artista>
        {
            new Artista {Id = id, Nome =  nomeArtista, IdGenero= idGenero},
            new Artista {Id = 2, Nome =  "artista dois", IdGenero = 2}
        };

            repositoryArtista.Setup(repository => repository.List()).ReturnsAsync(artistas);

            List<Artista> albunsRetornados = await _IServiceArtista.List();

            Assert.Equal(artistas, albunsRetornados);
        }
    }
}
