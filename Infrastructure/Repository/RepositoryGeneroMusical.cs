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

        public async Task Add(GeneroMusical GeneroMusical)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("INSERT INTO GeneroMusical (Nome) VALUES (@nome)",connection)) 
                {
                    command.Parameters.AddWithValue("nome", GeneroMusical.Nome);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<GeneroMusical> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM GeneroMusical Where Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("Id", Id);

                    using (var reader = await command.ExecuteReaderAsync()) 
                    {
                        if(await reader.ReadAsync())
                        {
                            return MapFromDataReader(reader);
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

                using (var command = new NpgsqlCommand("SELECT * FROM generomusical Where nome = @nome", connection))
                {
                    command.Parameters.AddWithValue("nome", NomeGeneroMusical);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapFromDataReader(reader);
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
                            GenerosMusicais.Add(MapFromDataReader(reader));
                        }
                        return GenerosMusicais;
                    }
                }
            }
        }

        public async Task Update(GeneroMusical GeneroMusical)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("UPDATE generomusical Set nome = @nome where Id = @id", connection))
                {
                    command.Parameters.AddWithValue("nome", GeneroMusical.Nome);
                    command.Parameters.AddWithValue("id", GeneroMusical.Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private GeneroMusical MapFromDataReader(IDataRecord reader)
        {
            return new GeneroMusical
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome"))
            };
        }
    }
}
