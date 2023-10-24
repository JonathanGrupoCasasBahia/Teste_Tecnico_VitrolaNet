using Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Album : Notifies
    {
        public int IdAlbum { get; set; }

        public string Nome { get; set; }

        public int AnoLancamento { get; set; }

        public int IdArtista { get; set; }

        public List<Musica> Musicas { get; set; }
    }
}
