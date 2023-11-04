using ApplicationApp.Interfaces;
using Domain.Interfaces;
using Entities.Entities;

namespace ApplicationApp.OpenApp
{
    public class MusicaApp : IMusicaApp
    {
        private readonly IServiceMusica _IServiceMusica;

        public MusicaApp(IServiceMusica IServiceMusica)
        {
            _IServiceMusica = IServiceMusica;
        }

        public async Task Add(string NomeMusica, int Ordem, int IdAlbum)
        {
            await _IServiceMusica.Add(NomeMusica, Ordem, IdAlbum);
        }

        public async Task Delete(int Id, int IdAlbum)
        {
            await _IServiceMusica.Delete(Id, IdAlbum);
        }

        public async Task<Musica> GetEntityByID(int Id)
        {
            return await _IServiceMusica.GetEntityByID(Id);
        }

        public async Task<List<Musica>> GetEntityByName(string TrechoNomeMusica)
        {
            return await _IServiceMusica.GetEntityByName(TrechoNomeMusica);
        }

        public async Task<List<Musica>> List()
        {
            return await _IServiceMusica.List();
        }

        public async Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum)
        {
            await _IServiceMusica.Update(Id, NovoNomeMusica, NovaOrdem, NovoIdAlbum);
        }
    }
}
