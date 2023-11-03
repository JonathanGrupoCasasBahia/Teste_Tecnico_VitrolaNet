using Entities.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repository
{
    public class RepositoryMusica : IRepositoryMusica
    {
        private readonly string _connectionString;

        public RepositoryMusica(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(string NomeMusica, int Ordem, int IdAlbum)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("INSERT INTO musica (nome,ordem,idAlbum) VALUES " +
                                                               "(@nomeMusica, @ordemMusica, @idDoAlbum)", connection))
                        {
                            command.Parameters.AddWithValue("nomeMusica", NomeMusica);
                            command.Parameters.AddWithValue("ordemMusica", Ordem);
                            command.Parameters.AddWithValue("idDoAlbum", IdAlbum);

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

        public async Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("UPDATE musica SET nome = @novoNome, ordem = @novaOrdem, idalbum = @novoIdAlbum " +
                                                                "WHERE id = @id", connection))
                        {
                            command.Parameters.AddWithValue("novoNome", NovoNomeMusica);
                            command.Parameters.AddWithValue("novaOrdem", NovaOrdem);
                            command.Parameters.AddWithValue("novoIdAlbum", NovoIdAlbum);
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

        public async Task Delete(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand("DELETE FROM musica WHERE id = @idMusica", connection))
                        {
                            command.Parameters.AddWithValue("idMusica", Id);

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

        public async Task<Musica> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using(var command = new NpgsqlCommand("SELECT * FROM musica WHERE id = @idMusica", connection))
                {
                    command.Parameters.AddWithValue("idMusica", Id);

                    using(var reader  = command.ExecuteReader())
                    {
                        Musica musica = null;
                        while (reader.Read())
                        {
                            musica = new Musica()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                Ordem = (int)reader["ordem"],
                                IdAlbum = (int)reader["idalbum"]
                            };
                        }return musica;
                    }
                }
            }
        }

        public async Task<List<Musica>> GetEntityByName(string TrechoNomeMusica)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM musica WHERE musica.nome LIKE '%' || @trechoNomeMusica || '%'", connection))
                {
                    command.Parameters.AddWithValue("trechoNomeMusica", TrechoNomeMusica);
                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        var Musicas = new List<Musica>();

                        while (await reader.ReadAsync())
                        {
                            Musica musica = new Musica()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                IdAlbum = (int)reader["idalbum"],
                                Ordem = (int)reader["ordem"]
                            };
                            Musicas.Add(musica);
                        }
                        return Musicas;
                    }
                }
                
            }
        }

        public async Task<List<Musica>> List()
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM musica", connection))
                {
                    using(var reader =  await command.ExecuteReaderAsync())
                    {
                        var  Musicas = new List<Musica>();

                        while (await reader.ReadAsync())
                        {
                            Musica musica = new Musica()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                IdAlbum = (int)reader["idalbum"],
                                Ordem = (int)reader["ordem"],
                            };
                            Musicas.Add(musica);
                        }
                        return Musicas;
                    }
                }
            }
        }

        public async Task<Musica> GetEntityByIDMusicaIdAlbum(int IdMusica, int IdAlbum)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using( var command = new NpgsqlCommand("SELECT * FROM musica WHERE musica.id = @idMusica AND musica.idalbum = @idAlbum", connection))
                {
                    command.Parameters.AddWithValue("idMusica", IdMusica);
                    command.Parameters.AddWithValue("idAlbum", IdAlbum);

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var Musica = new Musica()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                Ordem = (int)reader["ordem"],
                                IdAlbum = (int)reader["idalbum"]
                            };
                            return Musica;
                        }
                    }
                }
            }
            return null;
        }
    }
}
