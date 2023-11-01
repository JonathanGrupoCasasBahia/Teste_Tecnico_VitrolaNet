using Npgsql;

namespace Infrastructure.Configuration
{
    public class ContextBase
    {
        private readonly string _connectionString;

        public ContextBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InicializaDataBase()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    /*
                     * CREATE DATABASE "VitrolaNet"
                        WITH
                        OWNER = postgres
                        ENCODING = 'UTF8'
                        LC_COLLATE = 'Portuguese_Brazil.1252'
                        LC_CTYPE = 'Portuguese_Brazil.1252'
                        TABLESPACE = pg_default
                        CONNECTION LIMIT = -1
                        IS_TEMPLATE = False;
                    */ 

                    connection.ChangeDatabase("VitrolaNet");

                    string createTablesSql = @"
                    CREATE TABLE IF NOT EXISTS GeneroMusical (
                        Id serial PRIMARY KEY,
                        Nome character varying(20) NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS Artista (
                        Id serial PRIMARY KEY,
                        Nome character varying(20) NOT NULL,
                        IdGenero integer, 
                        FOREIGN KEY (IdGenero) REFERENCES GeneroMusical(Id)
                    );
                    CREATE TABLE IF NOT EXISTS Album (
                        Id serial PRIMARY KEY,
                        Nome character varying(20) NOT NULL,
                        AnoLancamento integer NOT NULL,
                        IdArtista integer,
                        FOREIGN KEY (IdArtista) REFERENCES Artista(Id)
                    );                    
                    CREATE TABLE IF NOT EXISTS Musica (
                        Id serial PRIMARY KEY,
                        Nome character varying(20) NOT NULL,
                        Ordem integer NOT NULL,
                        IdAlbum integer,
                        FOREIGN KEY (IdAlbum) REFERENCES Album(Id)
                    );
                ";
                    command.CommandText = createTablesSql;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
