using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Artista
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int IdGenero { get; set; }

        public GeneroMusical GeneroMusical { get; set; }

        public List<Album> Albums { get; set; }
    }
}
