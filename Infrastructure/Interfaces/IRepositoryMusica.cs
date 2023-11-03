using Entities.Entities;

namespace Infrastructure.Interfaces
{
    public interface IRepositoryMusica
    {
        Task Add(string NomeMusica, int Ordem, int IdAlbum);

        Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum);

        Task Delete(int Id);

        Task<Musica> GetEntityByID(int Id);

        Task<Musica> GetEntityByIDMusicaIdAlbum(int IdMusica, int IdAlbum);

        Task<List<Musica>> GetEntityByName(string TrechoNomeMusica);

        Task<List<Musica>> List();
    }
}
