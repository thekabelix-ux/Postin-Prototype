using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postin.Aplikacja
{

    public class Opinia
    {
        public Opinia()
        {
            data = DateTime.Now;
        }

        public int id_opinii { get; private set; }
        public DateTime data { get; private set; }
        public string opis { get; private set; }
    }
}
