﻿using Entities.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repository
{
    public class RepositoryAlbum : IRepositoryAlbum
    {
        private readonly string _connectionString;

        public RepositoryAlbum(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task Add(string NomeAlbum, int AnoLancamento, int IdArtista)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using(var command = new NpgsqlCommand("INSERT INTO album (nome, anolancamento, idartista) VALUES (@nomeAlbum, @anoLancamento, @idArtista)",connection))
                {
                    command.Parameters.AddWithValue("nomeAlbum", NomeAlbum);
                    command.Parameters.AddWithValue("anoLancamento", AnoLancamento);
                    command.Parameters.AddWithValue("idArtista", IdArtista);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(int IdAlbum,string NomeAlbum, int AnoLancamento, int IdArtista)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("UPDATE album SET " +
                                                       "nome = @nomeAlbum, anolancamento = @anoLancamento, idartista = @idArtista " +
                                                       "where id = @idAlbum", connection))
                {
                    command.Parameters.AddWithValue("idAlbum", IdAlbum);
                    command.Parameters.AddWithValue("nomeAlbum", NomeAlbum);
                    command.Parameters.AddWithValue("anoLancamento", AnoLancamento);
                    command.Parameters.AddWithValue("nomeAlbum", IdArtista);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int Id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using( var command = new NpgsqlCommand("DELETE from album where id = @id",connection))
                {
                    command.Parameters.AddWithValue("id", Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Album> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using( var command = new NpgsqlCommand("Select album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista as IdArtista, " +
                                                       "musica.nome as NomeMusica, musica.id as IdMusica, musica.ordem as Ordem " +
                                                       "from album " +
                                                       "Left Join musica ON album.id = musica.idAlbum " +
                                                       "where album.id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", Id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Album album = null;

                        while (await reader.ReadAsync())
                        {
                            if (album == null)
                            {
                                album = new Album()
                                {
                                    IdAlbum = (int)reader["id"],
                                    Nome = (string)reader["NomeAlbum"],
                                    AnoLancamento = (int)reader["AnoLancamento"],
                                    IdArtista = (int)reader["IdArtista"],
                                    Musicas = new List<Musica>()
                                };
                            }

                            if (reader["NomeMusica"] != DBNull.Value)
                            {
                                string musicaNome = (string)reader["NomeMusica"];
                                int idMusica = (int)reader["IdMusica"];
                                int ordem = (int)reader["Ordem"];
                                int idAlbum = (int)reader["id"];
                                album.Musicas.Add(new Musica { Id = idMusica, Nome = musicaNome, Ordem = ordem, IdAlbum = idAlbum });
                            }
                        }
                        return album;
                    }                    
                }
                    
            }
        }

        public async Task<List<Album>> GetEntityByName(string TrechoNomeAlbum)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("Select album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista as IdArtista, " +
                                                       "musica.nome as NomeMusica, musica.id as IdMusica, musica.ordem as Ordem " +
                                                       "from album " +
                                                       "LEFT JOIN musica ON album.id = musica.idAlbum " +
                                                       "where album.nome like '%' || @TrechoNomeAlbum ||'%'", connection))
                {
                    command.Parameters.AddWithValue("TrechoNomeAlbum", TrechoNomeAlbum);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var Albuns = new List<Album>();

                        while (await reader.ReadAsync())
                        {
                            Album album = new Album()
                            {
                                IdAlbum = (int)reader["id"],
                                Nome = (string)reader["NomeAlbum"],
                                AnoLancamento = (int)reader["AnoLancamento"],
                                IdArtista = (int)reader["IdArtista"],
                                Musicas = new List<Musica>()
                            };


                            if (reader["NomeMusica"] != DBNull.Value)
                            {
                                string musicaNome = (string)reader["NomeMusica"];
                                int idMusica = (int)reader["IdMusica"];
                                int ordem = (int)reader["Ordem"];
                                int idAlbum = (int)reader["id"];
                                album.Musicas.Add(new Musica { Id = idMusica, Nome = musicaNome, Ordem = ordem, IdAlbum = idAlbum });
                            }
                            Albuns.Add(album);
                        }
                        return Albuns;
                    }
                }

            }
        }

        public async Task<List<Album>> List()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("Select album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista as IdArtista, " +
                                                       "musica.nome as NomeMusica, musica.ordem as OrdemMusica, musica.id as idMusica, musica.idalbum as musicaIdAlbum " +
                                                       "FROM album " +
                                                       "LEFT JOIN musica ON album.id = musica.idalbum", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var Albuns = new List<Album>();

                        while (await reader.ReadAsync())
                        {
                            Album album = new Album()
                            {
                                IdAlbum = (int)reader["id"],
                                Nome = (string)reader["NomeAlbum"],
                                AnoLancamento = (int)reader["AnoLancamento"],
                                IdArtista = (int)reader["IdArtista"],
                                Musicas = new List<Musica>()
                            };

                            if (reader["NomeMusica"] != DBNull.Value)
                            {
                                string musicaNome = (string)reader["NomeMusica"];
                                int idMusica = (int)reader["IdMusica"];
                                int ordem = (int)reader["OrdemMusica"];
                                int idAlbum = (int)reader["id"];
                                album.Musicas.Add(new Musica { Id = idMusica, Nome = musicaNome, Ordem = ordem, IdAlbum = idAlbum });
                            }
                            Albuns.Add(album);
                        }
                        return Albuns;
                    }
                }

            }
        }

    }
}
