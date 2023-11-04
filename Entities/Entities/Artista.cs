namespace Entities.Entities
{
    public class Artista
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int IdGenero { get; set; }

        public string GeneroMusical { get; set; }

        public List<Album> Albuns { get; set; }
    }
}
