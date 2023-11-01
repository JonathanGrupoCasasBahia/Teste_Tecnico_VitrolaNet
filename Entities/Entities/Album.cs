namespace Entities.Entities
{
    public class Album 
    {
        public int IdAlbum { get; set; }

        public string Nome { get; set; }

        public int AnoLancamento { get; set; }

        public int IdArtista { get; set; }

        public List<string> Musicas { get; set; }
    }
}
