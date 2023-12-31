﻿using Entities.Entities;

namespace ApplicationApp.Interfaces
{
    public interface IMusicaApp
    {
        Task Add(string NomeMusica, int Ordem, int IdAlbum);

        Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum);

        Task Delete(int Id, int IdAlbum);

        Task<Musica> GetEntityByID(int Id);

        Task<List<Musica>> GetEntityByName(string TrechoNomeMusica);

        Task<List<Musica>> List();
    }
}
