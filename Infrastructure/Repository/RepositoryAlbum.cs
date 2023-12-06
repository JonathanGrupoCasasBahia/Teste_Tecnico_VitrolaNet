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

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("INSERT INTO album (nome, anolancamento, idartista) VALUES (@nomeAlbum, @anoLancamento, @idArtista)", connection))
                        {
                            command.Parameters.AddWithValue("nomeAlbum", NomeAlbum);
                            command.Parameters.AddWithValue("anoLancamento", AnoLancamento);
                            command.Parameters.AddWithValue("idArtista", IdArtista);

                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task Update(int IdAlbum,string NomeAlbum, int AnoLancamento, int IdArtista)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("UPDATE album SET " +
                                                               "nome = @nomeAlbum, anolancamento = @anoLancamento, idartista = @idArtista " +
                                                               "WHERE id = @idAlbum", connection))
                        {
                            command.Parameters.AddWithValue("idAlbum", IdAlbum);
                            command.Parameters.AddWithValue("nomeAlbum", NomeAlbum);
                            command.Parameters.AddWithValue("anoLancamento", AnoLancamento);
                            command.Parameters.AddWithValue("nomeAlbum", IdArtista);

                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }catch(Exception)
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

                        using (var command = new NpgsqlCommand("DELETE FROM album WHERE id = @id", connection))
                        {
                            command.Parameters.AddWithValue("id", Id);

                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }catch (Exception) 
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<Album> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using( var command = new NpgsqlCommand("SELECT album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista as IdArtista, " +
                                                       "musica.nome as NomeMusica, musica.id as IdMusica, musica.ordem as Ordem, musica.idalbum as musicaIdAlbum " +
                                                       "FROM album " +
                                                       "LEFT JOIN musica ON album.id = musica.idAlbum " +
                                                       "WHERE album.id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", Id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var albumDicionario = new Dictionary<int, Album>();
                        Album album = null;

                        while (await reader.ReadAsync())
                        {
                            int idAlbum = (int)reader["id"];

                            if (!albumDicionario.TryGetValue(idAlbum, out album))
                            {
                                album = new Album()
                                {
                                    IdAlbum = idAlbum,
                                    Nome = (string)reader["NomeAlbum"],
                                    AnoLancamento = (int)reader["AnoLancamento"],
                                    IdArtista = (int)reader["IdArtista"],
                                    Musicas = new List<Musica>()
                                };
                                albumDicionario.Add(album.IdAlbum, album);
                            }                                

                            if (reader["NomeMusica"] != DBNull.Value)
                            {
                                string musicaNome = (string)reader["NomeMusica"];
                                int idMusica = (int)reader["IdMusica"];
                                int ordem = (int)reader["Ordem"];
                                int musicaIdAlbum = (int)reader["musicaIdAlbum"];
                                album.Musicas.Add(new Musica { Id = idMusica, Nome = musicaNome, Ordem = ordem, IdAlbum = musicaIdAlbum });
                            }                            
                        }
                        return album;
                    }
                }
            }
        }

        public async Task<List<Album>> GetEntityByName(string TrechoNomeAlbum)
        {
            //TODO Dica: Usar um componente de conexão que atenda a vários tipos de BD. Caso haja mudança de SGBD não seria necessário mudar na aplicação, somente no config.
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista as IdArtista, " +
                                                       "musica.nome as NomeMusica, musica.id as IdMusica, musica.ordem as Ordem, musica.idalbum as musicaIdAlbum " +
                                                       "FROM album " +
                                                       "LEFT JOIN musica ON album.id = musica.idAlbum " +
                                                       "WHERE album.nome LIKE '%' || @TrechoNomeAlbum ||'%'", connection))
                {
                    command.Parameters.AddWithValue("TrechoNomeAlbum", TrechoNomeAlbum);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dicionarioAlbum = new Dictionary<int, Album>();
                        Album album;

                        while (await reader.ReadAsync())
                        {
                            int idAlbum = (int)reader["id"];

                            if(!dicionarioAlbum.TryGetValue(idAlbum, out album))
                            {
                                album = new Album()
                                {
                                    IdAlbum = idAlbum,
                                    Nome = (string)reader["NomeAlbum"],
                                    AnoLancamento = (int)reader["AnoLancamento"],
                                    IdArtista = (int)reader["IdArtista"],
                                    Musicas = new List<Musica>()
                                };
                                dicionarioAlbum.Add(idAlbum, album);
                            }

                            if (reader["NomeMusica"] != DBNull.Value)
                            {
                                string musicaNome = (string)reader["NomeMusica"];
                                int idMusica = (int)reader["IdMusica"];
                                int ordem = (int)reader["Ordem"];
                                int musicaIdAlbum = (int)reader["musicaIdAlbum"];
                                album.Musicas.Add(new Musica { Id = idMusica, Nome = musicaNome, Ordem = ordem, IdAlbum = musicaIdAlbum });
                            }
                        }
                        return dicionarioAlbum.Values.ToList();
                    }
                }
            }
        }

        public async Task<List<Album>> List()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT album.id, album.nome as NomeAlbum, album.anolancamento as AnoLancamento, album.idartista as IdArtista, " +
                                                       "musica.nome as NomeMusica, musica.ordem as OrdemMusica, musica.id as idMusica, musica.idalbum as musicaIdAlbum " +
                                                       "FROM album " +
                                                       "LEFT JOIN musica ON album.id = musica.idalbum", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dicionarioAlbum = new Dictionary<int, Album>();
                        Album album;

                        while (await reader.ReadAsync())
                        {
                            int idAlbum = (int)reader["id"];

                            if (!dicionarioAlbum.TryGetValue(idAlbum, out album))
                            {
                                album = new Album()
                                {
                                    IdAlbum = idAlbum,
                                    Nome = (string)reader["NomeAlbum"],
                                    AnoLancamento = (int)reader["AnoLancamento"],
                                    IdArtista = (int)reader["IdArtista"],
                                    Musicas = new List<Musica>()
                                };
                                dicionarioAlbum.Add(idAlbum, album);
                            }

                            if (reader["NomeMusica"] != DBNull.Value)
                            {
                                string musicaNome = (string)reader["NomeMusica"];
                                int idMusica = (int)reader["IdMusica"];
                                int ordem = (int)reader["OrdemMusica"];
                                int musicaIdAlbum = (int)reader["id"];

                                album.Musicas.Add(new Musica { Id = idMusica, Nome = musicaNome, Ordem = ordem, IdAlbum = musicaIdAlbum });
                            }                            
                        }
                        return dicionarioAlbum.Values.ToList();
                    }                    
                }

            }
        }

    }
}
