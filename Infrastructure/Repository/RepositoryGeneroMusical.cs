using Entities.Entities;
using Infrastructure.Interfaces;
using Npgsql;
using System.Data;

namespace Infrastructure.Repository
{
    public class RepositoryGeneroMusical : IRepositoryGeneroMusical
    {
        private readonly string _connectionString;

        public RepositoryGeneroMusical(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(string Nome)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("INSERT INTO GeneroMusical (Nome) VALUES (@nome)",connection)) 
                {
                    command.Parameters.AddWithValue("nome",Nome);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<GeneroMusical> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM GeneroMusical WHERE Id = @IdGenero", connection))
                {
                    command.Parameters.AddWithValue("IdGenero", Id);

                    using (var reader = await command.ExecuteReaderAsync()) 
                    {
                        if(await reader.ReadAsync())
                        {
                            GeneroMusical generoMusical = new GeneroMusical()
                            {
                                Id = (int)reader["Id"],
                                Nome = (string)reader["nome"]
                            };
                            return generoMusical;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<GeneroMusical> GetEntityByName(string NomeGeneroMusical)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM generomusical WHERE nome = @nomegenero", connection))
                {
                    command.Parameters.AddWithValue("nomegenero", NomeGeneroMusical);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            GeneroMusical generoMusical = new GeneroMusical()
                            {
                                Id = (int)reader["Id"],
                                Nome = (string)reader["nome"]
                            };
                            return generoMusical;
                        }
                    }
                }
            }
            return null;

        }

        public async Task<List<GeneroMusical>> List()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM GeneroMusical", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var GenerosMusicais = new List<GeneroMusical>();
                        
                        while (await reader.ReadAsync())
                        {
                            GeneroMusical generoMusical = new GeneroMusical()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"]
                            };

                            GenerosMusicais.Add(generoMusical);
                        }
                        return GenerosMusicais;
                    }
                }
            }
        }

        public async Task Update(int Id, string NovoNome)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("UPDATE generomusical SET nome = @nome WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("nome", NovoNome);
                    command.Parameters.AddWithValue("id", Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
