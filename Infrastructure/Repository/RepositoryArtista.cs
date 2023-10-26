using Entities.Entities;
using Infrastructure.Interfaces;
using Npgsql;
using System.Data;

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

                using (var command = new NpgsqlCommand("INSERT INTO artista (nome, idgenero) VALUES (@nome,@idgenero)"))
                {
                    command.Parameters.AddWithValue("nome", NomeArtista);
                    command.Parameters.AddWithValue("idgenero", IdGenero);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int Id)
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("DELETE artista where idartista = @idartista"))
                {
                    command.Parameters.AddWithValue("idartista", Id);

                    await command.ExecuteNonQueryAsync();
                }
                
            }
        }

        public async Task<Artista> GetEntityByID(int Id)
        {
            using(var connection = new Npgsql.NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using( var command = new NpgsqlCommand("SELECT * FROM artista where idartista = @id"))
                {
                    command.Parameters.AddWithValue("id", Id);

                    using( var reader = await command.ExecuteReaderAsync())
                    {
                        if(reader.Read())
                        {
                            return MapFromDataReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<Artista> GetEntityByName(string TrechoNome)
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT artista.id, artista.nome as nomeArtista, generomusical.nome as GeneroMusical, album.nome as Nomealbum"+
                                                       "FROM artista"+
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusica.id"+
                                                       "LEFT JOIN album ON artista.id = album.idartista"+
                                                       "where artista.nome LIKE @trechoNome"))
                {
                    command.Parameters.AddWithValue("trechoNome", TrechoNome);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Artista artista = null;

                        while(reader.Read())
                        {
                            if(artista == null)
                            {
                                artista = new Artista()
                                {
                                    Id = (int)reader["Id"],
                                    Nome = (string)reader["nomeArtista"],
                                    GeneroMusical = (GeneroMusical)reader["GeneroMusical"],
                                    Albuns = new List<Album>()
                                };
                            }
                            string albumNome = reader["Nomealbum"] as string;
                            if (!string.IsNullOrWhiteSpace(albumNome))
                            {
                                artista.Albuns.Add(new Album { Nome = albumNome});
                            }
                        }
                        return artista;
                    }
                }
            }
            return null;
        }

        public async Task<List<Artista>> List()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using(var command = new NpgsqlCommand("SELECT * FROM artista"))
                {
                    using( var reader = await command.ExecuteReaderAsync())
                    {
                        var Artistas = new List<Artista>();

                        while (reader.Read())
                        {
                            Artistas.Add(MapFromDataReader(reader));
                        }
                        return Artistas;
                    }
                }
            }
        }

        public async Task Update(int Id, string NovoNomeArtista, int NovoIdGenero)
        {
            using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("UPDATE artista SET nome = @novoNome, idgenero = @novoGenero where id = @id"))
                {
                    command.Parameters.AddWithValue("novoNome", NovoNomeArtista);
                    command.Parameters.AddWithValue("novoIdGenero", NovoIdGenero);
                    command.Parameters.AddWithValue("id", Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private Artista MapFromDataReader(IDataRecord reader)
        {
            return new Artista
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                IdGenero = reader.GetInt32(reader.GetOrdinal("IdGenero"))
            };
        }
    }
}
