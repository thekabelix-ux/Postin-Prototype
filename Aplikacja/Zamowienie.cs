using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postin.Aplikacja
{
    public class Zamowienie : Postin.Zewnetrzne.IDataRekord
    {
        public Zamowienie()
        {
            data_zlozenia = DateTime.Now;
        }

        public int numer { get; private set; }
        public float waga_laczna { get; private set; }
        public DateTime data_zlozenia { get; private set; }
        public string adres_dostawy { get; private set; }
        public string status { get; private set; }

        public List<Przesylka> przesylki { get; private set; }
    }

    public class Przesylka : Postin.Zewnetrzne.IDataRekord
    {
        public string idPrzesylki { get; private set; }
        public float waga { get; private set; }
        public float szerkosc { get; private set; }
        public float wysokosc { get; private set; }
        public float dlugosc { get; private set; }
        public string status { get; private set; }
        public bool czyDelikatna { get; private set; }
    }

    public class Zwrot : Postin.Zewnetrzne.IDataRekord
    {
        static private int index = 1;

        public Zwrot(string przyczyna)
        {
            data = DateTime.Now;
            idZwrotu = index++;
            this.przyczyna = przyczyna;
        }

        public int idZwrotu { get; private set; }
        public DateTime data {  get; private set; }
        public string przyczyna { get; private set; }
        public List<Przesylka> przesylki { get; private set; }
    }

}
