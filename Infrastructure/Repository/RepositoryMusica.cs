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
                using( var command = new NpgsqlCommand("UPDATE from musica SET nome = @novoNome, ordem = @novaOrdem, idalbum = @novoIdAlbum)", connection))
                {
                    command.Parameters.AddWithValue("novoNome", NovoNomeMusica);
                    command.Parameters.AddWithValue("novaOrdem", NovaOrdem);
                    command.Parameters.AddWithValue("novoIdAlbum", NovoIdAlbum);

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
                                IdAlbum = (int)reader["idalbum"]
                            };
                        }return musica;
                    }
                }
            }
        }

        public Task<List<Musica>> GetEntityByName(string TrechoNomeMusica)
        {
            throw new NotImplementedException();
        }

        public Task<List<Musica>> List()
        {
            throw new NotImplementedException();
        }
    }
}
