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
                using (var command = new NpgsqlCommand("INSERT INTO musica (nome,ordem,idAlbum) VALUES " +
                                                       "(@nomeMusica, @ordemMusica, @idDoAlbum)", connection))
                {
                    command.Parameters.AddWithValue("nomeMusica", NomeMusica);
                    command.Parameters.AddWithValue("ordemMusica", Ordem);
                    command.Parameters.AddWithValue("idDoAlbum", IdAlbum);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(int Id, string NovoNomeMusica, int NovaOrdem, int NovoIdAlbum)
        {
            using( var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using( var command = new NpgsqlCommand("UPDATE musica SET nome = @novoNome, ordem = @novaOrdem, idalbum = @novoIdAlbum " +
                                                       "where id = @id", connection))
                {
                    command.Parameters.AddWithValue("novoNome", NovoNomeMusica);
                    command.Parameters.AddWithValue("novaOrdem", NovaOrdem);
                    command.Parameters.AddWithValue("novoIdAlbum", NovoIdAlbum);
                    command.Parameters.AddWithValue("id", Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using( var command = new NpgsqlCommand("DELETE from musica where id = @idMusica", connection))
                {
                    command.Parameters.AddWithValue("idMusica", Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Musica> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using(var command = new NpgsqlCommand("select * from musica where id = @idMusica", connection))
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
                using (var command = new NpgsqlCommand("Select * from musica where musica.nome like '%' || @trechoNomeMusica || '%'", connection))
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
                using (var command = new NpgsqlCommand("select * from musica", connection))
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
    }
}
