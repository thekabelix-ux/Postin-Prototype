using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postin.Zewnetrzne;

namespace Postin.Aplikacja
{
    public class Plan_Dostawy : IDataRekord
    {
        static private int index = 1;
        private int id_planu;
        public DateTime data_dostawy { private set; get; }

        public List<Trasa> trasy { get; private set; }

        public Plan_Dostawy()
        {
            id_planu = index++;
            data_dostawy = DateTime.Now;
            trasy = new List<Trasa>();
        }

        public int GetIndex()
        {
            return id_planu;
        }
    }

    public class Trasa : IDataRekord
    {
        static private int id = 1;
        private int id_trasy;
        private int id_kuriera; 

        public List<Zamowienie> zamowienia { get; private set; } 

        public Trasa(int kurier)
        {
            id_trasy = id++;
            this.id_kuriera = kurier; 
            this.zamowienia = new List<Zamowienie>();
        }

        public int GetIdTrasy()
        {
            return id_trasy;
        }

        public int GetIdKuriera()
        {
            return id_kuriera;
        }
    }
}
