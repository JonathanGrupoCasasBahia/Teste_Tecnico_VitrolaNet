using Domain.Interfaces;
using Domain.Services;
using Entities.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repository;
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

        [Fact(DisplayName ="Não criar genero musical com mais de 20 caracteres")]
        public void AddGeneroMusicalInvalido() 
        {
            string nomeGenero = "Genero Musical Com Mais de Vinte Caracteres";

            Assert.ThrowsAsync<ArgumentException>(() => _IServiceGeneroMusical.Add(nomeGenero));
        }

        [Fact(DisplayName = "Não criar genero musical repetido")]
        public void AddGeneroMusicalRepetido()
        {
            string nomeGenero = "Samba";

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByName(nomeGenero)).ReturnsAsync(new GeneroMusical { Nome = nomeGenero });

            Assert.ThrowsAsync<InvalidOperationException>(() => _IServiceGeneroMusical.Add(nomeGenero));
        }

        [Fact(DisplayName = "criar genero musical")]
        public void AddGeneroMusical()
        {
            string nomeGenero = "Samba";

            repositoryGeneroMusical.Setup(repository => repository.GetEntityByName(nomeGenero)).ReturnsAsync((GeneroMusical)null);

            _IServiceGeneroMusical.Add(nomeGenero);

            repositoryGeneroMusical.Verify(repository => repository.Add(nomeGenero), Times.Once);
        }


    }
}
