using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Musica
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int Ordem { get; set; }

        public int IdAlbum { get; set; }

        public Album Album { get; set;}
    }
}
