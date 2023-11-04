using ApplicationApp.Interfaces;
using Domain.Interfaces;
using Entities.Entities;

namespace ApplicationApp.OpenApp
{
    public class AlbumApp : IAlbumApp
    {
        private readonly IServiceAlbum _IServiceAlbum;

        public AlbumApp(IServiceAlbum IServiceAlbum)
        {
            _IServiceAlbum = IServiceAlbum;
        }

        public async Task Add(string NomeAlbum, int AnoLancamentoAlbum, int IdArtista)
        {
            await _IServiceAlbum.Add(NomeAlbum, AnoLancamentoAlbum, IdArtista);
        }

        public async Task Delete(int Id)
        {
            await _IServiceAlbum.Delete(Id);
        }

        public async Task<Album> GetEntityByID(int Id)
        {
            return await _IServiceAlbum.GetEntityByID(Id);
        }

        public async Task<List<Album>> GetEntityByName(string NomeAlbum)
        {
            return await _IServiceAlbum.GetEntityByName(NomeAlbum);
        }

        public async Task<List<Album>> List()
        {
            return await _IServiceAlbum.List();
        }

        public async Task Update(int IdAlbum, string NovoNomeAlbum, int NovoAnoLancamentoAlbum, int NovoIdArtista)
        {
            await _IServiceAlbum.Update(IdAlbum, NovoNomeAlbum, NovoAnoLancamentoAlbum, NovoIdArtista);
        }
    }
}
