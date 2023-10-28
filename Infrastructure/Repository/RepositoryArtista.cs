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

                using (var command = new NpgsqlCommand("INSERT INTO artista (nome, idgenero) VALUES (@nome,@idgenero)", connection))
                {
                    command.Parameters.AddWithValue("nome", NomeArtista);
                    command.Parameters.AddWithValue("idgenero", IdGenero);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int Id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("DELETE from artista where id = @idartista", connection))
                {
                    command.Parameters.AddWithValue("idartista", Id);

                    await command.ExecuteNonQueryAsync();
                }                
            }
        }

        public async Task<Artista> GetEntityByID(int Id)
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using( var command = new NpgsqlCommand("SELECT artista.id, artista.nome as nomeArtista, " +
                                                       "generomusical.id as idGenero ,generomusical.nome as GeneroMusical, " +
                                                       "album.nome as Nomealbum " +
                                                       "FROM artista " +
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusical.id " +
                                                       "LEFT JOIN album ON artista.id = album.idartista " +
                                                       "where artista.id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", Id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Artista artista = null;

                        while (await reader.ReadAsync())
                        {
                            if (artista == null)
                            {
                                artista = new Artista()
                                {
                                    Id = (int)reader["id"],
                                    Nome = (string)reader["nomeArtista"],
                                    GeneroMusical = (string)reader["GeneroMusical"],
                                    IdGenero = (int)reader["idgenero"],
                                    Albuns = new List<Album>()
                                };
                            }
                            string albumNome = reader["Nomealbum"] as string;

                            if (!string.IsNullOrWhiteSpace(albumNome))
                            {
                                artista.Albuns.Add(new Album { Nome = albumNome });
                            }
                        }
                        return artista;
                    }
                }
            }
        }

        public async Task<List<Artista>> GetEntityByName(string TrechoNome)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT artista.id, artista.nome as nomeArtista, generomusical.id as idGenero ,generomusical.nome as GeneroMusical, album.nome as Nomealbum "+
                                                       "FROM artista "+
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusical.id "+
                                                       "LEFT JOIN album ON artista.id = album.idartista "+
                                                       "where artista.nome LIKE '%' || @trechoNome || '%'", connection))
                {
                    command.Parameters.AddWithValue("trechoNome", TrechoNome);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var Artistas = new List<Artista>();

                        while (await reader.ReadAsync())
                        {
                            Artista artista = new Artista()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nomeArtista"],
                                GeneroMusical = (string)reader["GeneroMusical"],
                                IdGenero = (int)reader["idgenero"],
                                Albuns = new List<Album>()
                            };

                            string albumNome = reader["Nomealbum"] as string;
                            if (!string.IsNullOrWhiteSpace(albumNome))
                            {
                                artista.Albuns.Add(new Album { Nome = albumNome });
                            }
                            Artistas.Add(artista);
                        }
                        return Artistas;
                    }
                }
            }
            
        }

        public async Task<List<Artista>> List()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT artista.id, artista.nome as nomeArtista, generomusical.id as idGenero ,generomusical.nome as GeneroMusical, album.nome as Nomealbum " +
                                                       "FROM artista " +
                                                       "INNER JOIN generomusical ON artista.idgenero = generomusical.id " +
                                                       "LEFT JOIN album ON artista.id = album.idartista", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var Artistas = new List<Artista>();

                        while (await reader.ReadAsync())
                        {
                            Artista artista = new Artista()
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nomeArtista"],
                                GeneroMusical = (string)reader["GeneroMusical"],
                                IdGenero = (int)reader["idgenero"],
                                Albuns = new List<Album>()
                            };

                            string albumNome = reader["Nomealbum"] as string;
                            if (!string.IsNullOrWhiteSpace(albumNome))
                            {
                                artista.Albuns.Add(new Album { Nome = albumNome });
                            }
                            Artistas.Add(artista);
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

                using (var command = new NpgsqlCommand("UPDATE artista SET nome = @novoNome, idgenero = @novoIdGenero where id = @id", connection))
                {
                    command.Parameters.AddWithValue("novoNome", NovoNomeArtista);
                    command.Parameters.AddWithValue("novoIdGenero", NovoIdGenero);
                    command.Parameters.AddWithValue("id", Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
