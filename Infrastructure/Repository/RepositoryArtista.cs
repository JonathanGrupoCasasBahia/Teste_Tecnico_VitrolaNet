using Entities.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repository
{
    public class RepositoryArtista : IRepositoryArtista
    {
        private readonly string _connectionString;
        public RepositoryArtista(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(string NomeArtista, int IdGenero)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("INSERT INTO artista (nome, idgenero) VALUES (@nome,@idgenero)", connection))
                        {
                            command.Parameters.AddWithValue("nome", NomeArtista);
                            command.Parameters.AddWithValue("idgenero", IdGenero);

                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task Delete(int Id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("DELETE FROM artista WHERE id = @idartista", connection))
                        {
                            command.Parameters.AddWithValue("idartista", Id);

                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<Artista> GetEntityByID(int Id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT artista.id as idArtista, artista.nome as nomeArtista, " +
                                                       "generomusical.id as idGenero ,generomusical.nome as GeneroMusical, " +
                                                       "album.nome as Nomealbum , album.id as idAlbum, album.anoLancamento as AnoLancamentoAlbum , album.idartista as albumIdArtista, " +
                                                       "musica.id as idMusica, musica.nome as nomeMusica, musica.ordem as ordemMusica, musica.idalbum as musicaIdAlbum " +
                                                       "FROM artista " +
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusical.id " +
                                                       "LEFT JOIN album ON artista.id = album.idartista " +
                                                       "LEFT JOIN musica ON album.id = musica.idalbum " +
                                                       "WHERE artista.id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", Id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dicionarioAlbum = new Dictionary<int, Album>();

                        while (await reader.ReadAsync())
                        {
                            Artista artista = new Artista()
                            {
                                Id = (int)reader["idArtista"],
                                Nome = (string)reader["nomeArtista"],
                                GeneroMusical = (string)reader["GeneroMusical"],
                                IdGenero = (int)reader["idgenero"],
                                Albuns = new List<Album>()
                            };

                            if (reader["Nomealbum"] != DBNull.Value)
                            {
                                int idAlbum = (int)reader["idAlbum"];
                                string albumNome = (string)reader["Nomealbum"];
                                int anoLancamento = (int)reader["AnoLancamentoAlbum"];
                                int albumIdArtista = (int)reader["albumIdArtista"];

                                if (!dicionarioAlbum.TryGetValue(idAlbum, out Album album))
                                {
                                    album = new Album
                                    {
                                        IdAlbum = idAlbum,
                                        Nome = albumNome,
                                        AnoLancamento = anoLancamento,
                                        IdArtista = albumIdArtista,
                                        Musicas = new List<Musica>()
                                    };
                                    dicionarioAlbum.Add(idAlbum, album);
                                }

                                if (reader["nomeMusica"] != DBNull.Value)
                                {
                                    int idMusica = (int)reader["idMusica"];
                                    string nomeMusica = (string)reader["nomeMusica"];
                                    int ordemMusica = (int)reader["ordemMusica"];
                                    int musicaIdAlbum = (int)reader["musicaIdAlbum"];

                                    album.Musicas.Add(new Musica { Id = idMusica, Nome = nomeMusica, Ordem = ordemMusica, IdAlbum = musicaIdAlbum });
                                }

                                artista.Albuns.Add(album);
                            }
                            return artista;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<Artista>> GetEntityByName(string TrechoNome)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT artista.id as idArtista, artista.nome as nomeArtista, " +
                                                       "generomusical.id as idGenero ,generomusical.nome as GeneroMusical, " +
                                                       "album.nome as Nomealbum , album.id as idAlbum, album.anoLancamento as AnoLancamentoAlbum , album.idartista as albumIdArtista, " +
                                                       "musica.id as idMusica, musica.nome as nomeMusica, musica.ordem as ordemMusica, musica.idalbum as musicaIdAlbum " +
                                                       "FROM artista " +
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusical.id " +
                                                       "LEFT JOIN album ON artista.id = album.idartista " +
                                                       "LEFT JOIN musica ON musica.idalbum = album.id " +
                                                       "WHERE artista.nome LIKE '%' || @trechoNome || '%'", connection))
                {
                    command.Parameters.AddWithValue("trechoNome", TrechoNome);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dicionarioArtista = new Dictionary<int, Artista>();

                        Artista artista;

                        while (await reader.ReadAsync())
                        {
                            int idArtista = (int)reader["idArtista"];

                            if (!dicionarioArtista.TryGetValue(idArtista, out artista))
                            {
                                artista = new Artista()
                                {
                                    Id = idArtista,
                                    Nome = (string)reader["nomeArtista"],
                                    GeneroMusical = (string)reader["GeneroMusical"],
                                    IdGenero = (int)reader["idgenero"],
                                    Albuns = new List<Album>()
                                };

                                dicionarioArtista.Add(idArtista, artista);
                            }

                            if (reader["Nomealbum"] != DBNull.Value)
                            {
                                int idAlbum = (int)reader["idAlbum"];
                                string albumNome = (string)reader["Nomealbum"];
                                int anoLancamento = (int)reader["AnoLancamentoAlbum"];
                                int albumIdArtista = (int)reader["albumIdArtista"];

                                var album = artista.Albuns.FirstOrDefault(album => album.IdAlbum == idAlbum);

                                if (album == null)
                                {
                                    album = new Album
                                    {
                                        IdAlbum = idAlbum,
                                        Nome = albumNome,
                                        AnoLancamento = anoLancamento,
                                        IdArtista = albumIdArtista,
                                        Musicas = new List<Musica>()
                                    };

                                    if (reader["nomeMusica"] != DBNull.Value)
                                    {
                                        int idMusica = (int)reader["idMusica"];
                                        string nomeMusica = (string)reader["nomeMusica"];
                                        int ordemMusica = (int)reader["ordemMusica"];
                                        int musicaIdAlbum = (int)reader["musicaIdAlbum"];

                                        album.Musicas.Add(new Musica { Id = idMusica, Nome = nomeMusica, Ordem = ordemMusica, IdAlbum = musicaIdAlbum });
                                    }
                                    artista.Albuns.Add(album);
                                }
                                else if (reader["nomeMusica"] != DBNull.Value)
                                {
                                    int idMusica = (int)reader["idMusica"];
                                    string nomeMusica = (string)reader["nomeMusica"];
                                    int ordemMusica = (int)reader["ordemMusica"];
                                    int musicaIdAlbum = (int)reader["musicaIdAlbum"];

                                    album.Musicas.Add(new Musica { Id = idMusica, Nome = nomeMusica, Ordem = ordemMusica, IdAlbum = musicaIdAlbum });
                                }
                            }
                        }
                        return dicionarioArtista.Values.ToList();
                    }
                }
            }
        }

        public async Task<List<Artista>> List()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT artista.id as idArtista, artista.nome as nomeArtista, " +
                                                       "generomusical.id as idGenero ,generomusical.nome as GeneroMusical, " +
                                                       "album.nome as Nomealbum , album.id as idAlbum, album.anoLancamento as AnoLancamentoAlbum , album.idartista as albumIdArtista, " +
                                                       "musica.id as idMusica, musica.nome as nomeMusica, musica.ordem as ordemMusica, musica.idalbum as musicaIdAlbum " +
                                                       "FROM artista " +
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusical.id " +
                                                       "LEFT JOIN album ON artista.id = album.idartista " +
                                                       "LEFT JOIN musica ON musica.idalbum = album.id", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dicionarioArtista = new Dictionary<int, Artista>();
                        Artista artista;

                        while (await reader.ReadAsync())
                        {
                            int idArtista = (int)reader["idArtista"];

                            if (!dicionarioArtista.TryGetValue(idArtista, out artista))
                            {
                                artista = new Artista()
                                {
                                    Id = idArtista,
                                    Nome = (string)reader["nomeArtista"],
                                    GeneroMusical = (string)reader["GeneroMusical"],
                                    IdGenero = (int)reader["idgenero"],
                                    Albuns = new List<Album>()
                                };

                                dicionarioArtista.Add(idArtista, artista);
                            }

                            if (reader["Nomealbum"] != DBNull.Value)
                            {
                                int idAlbum = (int)reader["idAlbum"];
                                string albumNome = (string)reader["Nomealbum"];
                                int anoLancamento = (int)reader["AnoLancamentoAlbum"];

                                var album = artista.Albuns.FirstOrDefault(a => a.IdAlbum == idAlbum);

                                if (album == null)
                                {
                                    album = new Album
                                    {
                                        IdAlbum = idAlbum,
                                        Nome = albumNome,
                                        AnoLancamento = anoLancamento,
                                        IdArtista = idArtista,
                                        Musicas = new List<Musica>()
                                    };
                                    if (reader["nomeMusica"] != DBNull.Value)
                                    {
                                        int idMusica = (int)reader["idMusica"];
                                        string nomeMusica = (string)reader["nomeMusica"];
                                        int ordemMusica = (int)reader["ordemMusica"];
                                        int musicaIdAlbum = (int)reader["musicaIdAlbum"];

                                        album.Musicas.Add(new Musica { Id = idMusica, Nome = nomeMusica, Ordem = ordemMusica, IdAlbum = musicaIdAlbum });
                                    }
                                    artista.Albuns.Add(album);
                                }
                                else if (reader["nomeMusica"] != DBNull.Value.ToString())
                                {
                                    int idMusica = (int)reader["idMusica"];
                                    string nomeMusica = (string)reader["nomeMusica"];
                                    int ordemMusica = (int)reader["ordemMusica"];
                                    int musicaIdAlbum = (int)reader["musicaIdAlbum"];

                                    album.Musicas.Add(new Musica { Id = idMusica, Nome = nomeMusica, Ordem = ordemMusica, IdAlbum = musicaIdAlbum });
                                }
                            }
                        }
                        return dicionarioArtista.Values.ToList();
                    }
                }
            }
        }

        public async Task Update(int Id, string NovoNomeArtista, int NovoIdGenero)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("UPDATE artista SET nome = @novoNome, idgenero = @novoIdGenero WHERE id = @id", connection))
                        {
                            command.Parameters.AddWithValue("novoNome", NovoNomeArtista);
                            command.Parameters.AddWithValue("novoIdGenero", NovoIdGenero);
                            command.Parameters.AddWithValue("id", Id);

                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
