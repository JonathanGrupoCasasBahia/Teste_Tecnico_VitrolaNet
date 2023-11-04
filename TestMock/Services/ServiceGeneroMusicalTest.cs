using Domain.Interfaces;
using Domain.Services;
using Entities.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace TestMock.Services
{
    public class ServiceGeneroMusicalTest
    {
        private readonly IServiceGeneroMusical _IServiceGeneroMusical;
        private readonly Mock<IRepositoryGeneroMusical> repositoryGeneroMusical;

        public ServiceGeneroMusicalTest()
        {
            repositoryGeneroMusical = new Mock<IRepositoryGeneroMusical>();

            _IServiceGeneroMusical = new ServiceGeneroMusical(repositoryGeneroMusical.Object);
        }

        [Fact(DisplayName ="Nao criar genero musical com mais de 20 caracteres")]
        public async Task AddGeneroMusicalInvalido() 
        {
            string nomeGenero = "Genero Musical Com Mais de Vinte Caracteres";

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.Add(nomeGenero));

            Assert.Equal("Nome do gênero musical inválido.", exception.Message);
        }

        [Fact(DisplayName = "Nao criar genero musical repetido")]
        public async Task AddGeneroMusicalRepetido()
        {
            string nomeGenero = "Samba";

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByName(nomeGenero)).ReturnsAsync(new GeneroMusical { Nome = nomeGenero });

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.Add(nomeGenero));

            Assert.Equal("O gênero musical já existe no catálogo.", exception.Message);
        }

        [Fact(DisplayName = "criar genero musical")]
        public async Task AddGeneroMusical()
        {
            string nomeGenero = "Samba";

            GeneroMusical generoMusical = null;

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByName(nomeGenero)).ReturnsAsync(generoMusical);

            await _IServiceGeneroMusical.Add(nomeGenero);

            repositoryGeneroMusical.Verify(repository => repository.Add(nomeGenero), Times.Once);
        }


        //--------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName ="Nao atualizar genero musical, nome invalido")]
        public async Task UpdateGeneroMusicalInvalido()
        {
            string nomeGenero = "Genero Musical Com Mais de Vinte Caracteres";

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.Add(nomeGenero));

            Assert.Equal("Nome do gênero musical inválido.", exception.Message);
        }

        [Fact(DisplayName = "Nao atualizar genero musical anterior nao encontrado")]
        public async Task UpdateGeneroMusicalNaoEncontrado()
        {
            int id = 1;
            string nome = "nome valido";
            GeneroMusical generoMusical = null;


            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(generoMusical);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.Update(id, nome));

            Assert.Equal("O gênero musical não existe no catálogo.", exception.Message);
        }

        [Fact(DisplayName = "Nao atualizar genero musical novo genero ja existente")]
        public async Task UpdateGeneroMusicalDuplicado()
        {
            int id = 1;
            string nome = "nome valido";

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new GeneroMusical { Id = id});
            repositoryGeneroMusical.Setup(repository => repository.GetEntityByName(nome)).ReturnsAsync(new GeneroMusical { Id = id , Nome = nome});

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.Update(id, nome));

            Assert.Equal("O novo nome do gênero musical já existe no catálogo.", exception.Message);
        }

        [Fact(DisplayName = "Atualizar genero musical")]
        public async Task UpdateGeneroMusical()
        {
            int id = 1;
            string nomeAntigo = "nome valido antigo";
            string novoNome = "nome valido novo";

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new GeneroMusical { Id = id, Nome = nomeAntigo });

            await _IServiceGeneroMusical.Update(id, novoNome);

            repositoryGeneroMusical.Verify(repository => repository.Update(id, novoNome), Times.Once);
        }

        //---------------------------------------------------------------------------------------------------------------

        [Fact(DisplayName = "Nao achar id genero null")]
        public async Task getByIdNull()
        {
            int id = 20;
            GeneroMusical generoMusical = null;

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(generoMusical);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.GetEntityByID(id));   
            
            Assert.Equal("O gênero musical não existe no catálogo.", exception.Message);
        }

        [Fact(DisplayName = "Encontrar o genero pelo Id")]
        public async Task getByIdSuccess() 
        {
            int id = 1;
            string nome = "genero Valido";

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByID(id)).ReturnsAsync(new GeneroMusical { Id = id, Nome = nome });

            GeneroMusical generoRetornado =  await _IServiceGeneroMusical.GetEntityByID(id);

            Assert.Equal(id, generoRetornado.Id);

            Assert.Equal(nome, generoRetornado.Nome);
        }

        [Fact(DisplayName = "Listar generos musicais com sucesso")]
        public async Task ListGenerosMusicaisSuccess()
        {
            List<GeneroMusical> generos = new List<GeneroMusical>
            {
                new GeneroMusical { Id = 1, Nome = "Rock" },
                new GeneroMusical { Id = 2, Nome = "Pop" }
            };

            repositoryGeneroMusical.Setup(repository => repository.List()).ReturnsAsync(generos);

            List<GeneroMusical> generosRetornados = await _IServiceGeneroMusical.List();

            Assert.Equal(generos, generosRetornados);
        }

        [Fact(DisplayName = "Listar gêneros musicais (lista vazia)")]
        public async Task ListGenerosMusicaisEmptyList()
        {
            List<GeneroMusical> generos = new List<GeneroMusical>();

            repositoryGeneroMusical.Setup(repository => repository.List()).ReturnsAsync(generos);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.List());

            Assert.Equal("Lista de generos vazia", exception.Message);
        }

    }
}
