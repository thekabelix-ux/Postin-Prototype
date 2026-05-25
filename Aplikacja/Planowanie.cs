using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postin.Aplikacja
{
    public class Plan_Dostawy : Postin.Zewnetrzne.IDataRekord
    {
        static private int index = 1;
        private int id_planu;
        public DateTime data_dostawy { private set; get; }

        public Plan_Dostawy()
        {
            id_planu = index++;
            data_dostawy = DateTime.Now;
        }


        public int GetIndex()
        {
            return id_planu;
        }
    }

    public class Trasa : Postin.Zewnetrzne.IDataRekord
    {
        static private int id = 1;
        private int id_trasy;
        List<object> data;

        public Trasa(int kurier)
        {
            id_trasy = id++;
            this.id_kuriera = kurier;
        }

        private int id_kuriera;
    }
}
