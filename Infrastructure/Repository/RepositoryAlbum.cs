using Entities.Entities;
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

                using(var command = new NpgsqlCommand("INSERT INTO album (nome, anolancamento, idartista) VALUES (@nomeAlbum, @anoLancamento, @idArtista",connection))
                {
                    command.Parameters.AddWithValue("nomeAlbum", NomeAlbum);
                    command.Parameters.AddWithValue("anoLancamento", AnoLancamento);
                    command.Parameters.AddWithValue("nomeAlbum", IdArtista);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(string NomeAlbum, int AnoLancamento, int IdArtista)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("UPDATE album SET nome = @nomeAlbum, anolancamento = @anoLancamento, idartista = @idArtista", connection))
                {
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

                using( var command = new NpgsqlCommand("Select album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista = IdArtista" +
                                                       "musica.nome as NomeMusica" +
                                                       "from album" +
                                                       "LEFT JOIN musica ON album.id = musica.id" +
                                                       "where id = @id", connection))
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
                            string musicaNome = reader["NomeMusica"] as string;

                            if(!string.IsNullOrWhiteSpace(musicaNome))
                            {
                                album.Musicas.Add(new Musica() {Nome = musicaNome});
                            }
                        }return album;
                    }                    
                }
                    
            }
        }

        public async Task<List<Album>> GetEntityByName(string NomeAlbum)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("Select album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista = IdArtista" +
                                                       "musica.nome as NomeMusica" +
                                                       "from album" +
                                                       "LEFT JOIN musica ON album.id = musica.id" +
                                                       "where nome = @nomeAlbum", connection))
                {
                    command.Parameters.AddWithValue("nomeAlbum", NomeAlbum);

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

                            string musicaNome = reader["NomeMusica"] as string;
                            if (!string.IsNullOrWhiteSpace(musicaNome))
                            {
                                album.Musicas.Add(new Musica() { Nome = musicaNome });
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

                using (var command = new NpgsqlCommand("Select album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista = IdArtista" +
                                                       "musica.nome as NomeMusica" +
                                                       "from album" +
                                                       "LEFT JOIN musica ON album.id = musica.id", connection))
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

                            string musicaNome = reader["NomeMusica"] as string;
                            if (!string.IsNullOrWhiteSpace(musicaNome))
                            {
                                album.Musicas.Add(new Musica() { Nome = musicaNome });
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
