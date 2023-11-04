using Domain.Interfaces;
using Domain.Services;
using Entities.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace TestMock.Services
{
    public class ServiceMusicaTest
    {
        private readonly IServiceMusica _IServiceMusica;
        private readonly Mock<IRepositoryAlbum> repositoryAlbum;
        private readonly Mock<IRepositoryMusica> repositoryMusica;

        public ServiceMusicaTest()
        {
            repositoryAlbum = new Mock<IRepositoryAlbum>();
            repositoryMusica = new Mock<IRepositoryMusica>();

            _IServiceMusica = new ServiceMusica(repositoryMusica.Object,repositoryAlbum.Object);
        }

        [Fact(DisplayName = "Nao criar Musica com mais de 30 caracteres")]
        public async Task AddMusicaInvalido()
        {
            string nomeMusica = "Musica Com Mais de Vinte Caracteres nao eh permitido";
            int ordem = 1;
            int idAlbum = 2;

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.Add(nomeMusica, ordem, idAlbum));

            Assert.Equal("Nome da música inválido.", exception.Message);
        }
                
        [Fact(DisplayName = "Nao criar musica, album nao existe")]
        public async Task AddMusicaAlbumNaoExiste()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            Album album = null;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(album);
            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = idAlbum});

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.Add(nomeMusica, ordem, idAlbum));

            Assert.Equal("Album informado não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "criar Musica")]
        public async void AddMusica()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 2023;
            int idAlbum = 1;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(new Album { IdAlbum = idAlbum });
            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = idAlbum });

            await _IServiceMusica.Add(nomeMusica, ordem, idAlbum);

            repositoryMusica.Verify(repository => repository.Add(nomeMusica, ordem, idAlbum), Times.Once);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao atualizar musica com mais de 30 caracteres")]
        public async Task UpdateMusicaInvalido()
        {
            int id = 1;
            string nomeMusica = "Musica Com Mais de Vinte Caracteres nao eh permitido";
            int ordem = 1;
            int idAlbum = 1;

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.Update(id, nomeMusica, ordem, idAlbum));


            Assert.Equal("Nome da nova música inválido.", exception.Message);
        }
        
        [Fact(DisplayName = "Nao atualizar musica, ela nao existe")]
        public async Task UpdateMusicaNaoExiste()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 202;
            int idAlbum = 1;

            Musica musica = null;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(new Album { IdAlbum = idAlbum });
            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(musica);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.Update(id, nomeMusica, ordem, idAlbum));

            Assert.Equal("Musica não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Nao atualizar musica, album nao existe")]
        public async Task UpdateMusicaAlbumNaoExiste()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            Album album = null;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(album);
            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = idAlbum });

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.Update(id, nomeMusica, ordem, idAlbum));

            Assert.Equal("Album não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Atualizar Musica")]
        public async void UpdateMusica()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(new Album { IdAlbum = idAlbum });
            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = idAlbum });

            await _IServiceMusica.Update(id, nomeMusica, ordem, idAlbum);

            repositoryMusica.Verify(repository => repository.Update(id, nomeMusica, ordem, idAlbum), Times.Once);
        }
        
        //---------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao deletar musica, musica informada nao faz parte do album ou nao existe")]
        public async void DeleteMusicaNaoExiste()
        {
            int id = 1;
            int idAlbum = 1;

            Musica musica = null;

            repositoryMusica.Setup(reposytory => reposytory.GetEntityByIDMusicaIdAlbum(id,idAlbum)).ReturnsAsync((musica));
            

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.Delete(id, idAlbum));

            Assert.Equal("Musica não pertence ao album.", exception.Message);
        }
        
        [Fact(DisplayName = "Deletar somente a musica, pois o album tera outras musicas")]
        public async void DeleteSomenteMusica()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            List<Musica> musicas = new List<Musica>()
            {
                new Musica{Nome = nomeMusica, Id = id, IdAlbum = idAlbum, Ordem = ordem},
                new Musica{Nome = "musica dois", Id = 2, IdAlbum = 1, Ordem = ordem}
            };

            repositoryMusica.Setup(repository => repository.GetEntityByIDMusicaIdAlbum(id,idAlbum)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = id});
            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(new Album { IdAlbum = idAlbum , Musicas = musicas});

            await _IServiceMusica.Delete(id,idAlbum);

            repositoryMusica.Verify(repository => repository.Delete(id,idAlbum),Times.Once);
        }
        
        [Fact(DisplayName = "Deletar musica e album, pois eh a ultima musica")]
        public async void DeleteMusicaEAlbum()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            List<Musica> musicas = new List<Musica>()
            {
                new Musica{Nome = nomeMusica, Id = id, IdAlbum = idAlbum, Ordem = ordem}
            };

            repositoryMusica.Setup(repository => repository.GetEntityByIDMusicaIdAlbum(id,idAlbum)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = id});
            repositoryAlbum.Setup(repository => repository.GetEntityByID(idAlbum)).ReturnsAsync(new Album { IdAlbum = idAlbum , Musicas = musicas});

            await _IServiceMusica.Delete(id,idAlbum);

            repositoryMusica.Verify(repository => repository.Delete(id,idAlbum),Times.Once);
            repositoryAlbum.Verify(repository => repository.Delete(idAlbum), Times.Once);
        }
        
        //---------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao achar musica, nome null")]
        public async Task GetByNameNull()
        {
            string nome = "nome musica valido";

            List<Musica> musicas = new List<Musica>();

            repositoryMusica.Setup(repository => repository.GetEntityByName(nome)).ReturnsAsync((musicas));

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.GetEntityByName(nome));

            Assert.Equal("Musica não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Encontrar o musica pelo Nome")]
        public async Task GetByNameSuccess()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            List<Musica> musicas = new List<Musica>()
            {
                new Musica {Id = id, Nome =  nomeMusica, Ordem = ordem, IdAlbum = idAlbum}
            };

            repositoryMusica.Setup(repository => repository.GetEntityByName(nomeMusica)).ReturnsAsync(musicas);

            List<Musica> musicaRetornado = await _IServiceMusica.GetEntityByName(nomeMusica);

            Assert.Equal(musicas, musicaRetornado);
        }
        
        [Fact(DisplayName = "Nao achar musica, id null")]
        public async Task GetByIdNull()
        {
            int id = 20;
            Musica musica = null;

            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(musica);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.GetEntityByID(id));

            Assert.Equal("Musica não existe.", exception.Message);
        }
        
        [Fact(DisplayName = "Encontrar o musica pelo Id")]
        public async Task GetByIdSuccess()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            repositoryMusica.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new Musica { Id = id, Nome = nomeMusica, Ordem = ordem, IdAlbum = idAlbum });

            Musica musicaRetornado = await _IServiceMusica.GetEntityByID(id);

            Assert.Equal(id, musicaRetornado.Id);

            Assert.Equal(nomeMusica, musicaRetornado.Nome);

            Assert.Equal(ordem, musicaRetornado.Ordem);

            Assert.Equal(idAlbum, musicaRetornado.IdAlbum);
        }
        
        [Fact(DisplayName = "Listar musicas (lista vazia)")]
        public async Task ListarAlbunsEmptyList()
        {
            List<Musica> musicas = new List<Musica>();

            repositoryMusica.Setup(repository => repository.List()).ReturnsAsync(musicas);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceMusica.List());

            Assert.Equal("Lista de musicas vazia", exception.Message);
        }
        
        [Fact(DisplayName = "Listar musicas com sucesso")]
        public async Task ListAlbunsSuccess()
        {
            int id = 1;
            string nomeMusica = "Nome musica valido";
            int ordem = 1;
            int idAlbum = 1;

            List<Musica> musicas = new List<Musica>
            {
                new Musica {Id = id, Nome =  nomeMusica, Ordem = ordem, IdAlbum = idAlbum},
                new Musica {Id = 2, Nome =  "musica dois", Ordem = 2023, IdAlbum = 7}
            };

            repositoryMusica.Setup(repository => repository.List()).ReturnsAsync(musicas);

            List<Musica> albunsRetornados = await _IServiceMusica.List();

            Assert.Equal(musicas, albunsRetornados);
        }
    }
}
