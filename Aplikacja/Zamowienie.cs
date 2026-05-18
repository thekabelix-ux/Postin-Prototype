using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postin.Aplikacja
{
    public class Zamowienie
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

    public class Przesylka
    {
        public string id_przesylki { get; private set; }
        public float waga { get; private set; }
        public float szerkosc { get; private set; }
        public float wysokosc { get; private set; }
        public float dlugosc { get; private set; }
        public string status { get; private set; }
        public bool czy_delikatna { get; private set; }
    }

    public class Zwrot
    {
        public Zwrot()
        {
            data = DateTime.Now;
        }

        public int id_zwrotu { get; private set; }
        public DateTime data {  get; private set; }
        public string przyczyna { get; private set; }
        public List<Przesylka> przesylki { get; private set; }
    }

    public class Opinia
    {
        public Opinia()
        {
            data = DateTime.Now;
        }

        public int id_opinii { get; private set; }
        public DateTime data { get; private set; }
        public string opis {  get; private set; }
    }
}
