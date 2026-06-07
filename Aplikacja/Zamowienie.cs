using Postin;
using Postin.Zewnetrzne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Postin.Aplikacja
{
    public class Zamowienie : IDataRekord
    {
        public Zamowienie(string adres)
        {
            data_zlozenia = DateTime.Now;
            przesylki = new List<Przesylka>();
            historia = new List<string>(); 
            status = "Oczekiwanie na";      
            adres_dostawy = adres;
        }

        public int numer { get; private set; }
        public float waga_laczna { get; set; }
        public DateTime data_zlozenia { get; private set; }
        public string adres_dostawy { get; private set; }
        public string status { get; set; }
        public List<Przesylka> przesylki { get; private set; }
        public List<string> historia { get; private set; }
        public string szacowanyCzasDostawy { get; set; }
        public string idKuriera { get; set; } 
        public string kodQR { get; set; }  

        public void DodajKrokHistorii(string opisZdarzenia)
        {
            historia.Add(opisZdarzenia);
        }
    }

    public class Wymiary
    {
        public float szerokosc { get; private set; } // POPRAWKA: public i właściwości
        public float wysokosc { get; private set; }  
        public float dlugosc { get; private set; }

        public Wymiary(float a, float b, float c) // POPRAWKA: dodano public
        {
            szerokosc = a;
            wysokosc = b;
            dlugosc = c;
        }
    }

    public class Przesylka : IDataRekord
    {
        static int index = 0;
        public int idPrzesylki { get; private set; }
        public float waga { get; private set; }
        public float szerokosc { get; private set; }
        public float wysokosc { get; private set; }
        public float dlugosc { get; private set; }
        public string status { get; set; }
        public bool czyDelikatna { get; private set; }
        public string adres { get; private set; } 

        // Zwraca objętość w metrach sześciennych (cm -> m)
        public float ObjetoscM3 => (szerokosc / 100f) * (wysokosc / 100f) * (dlugosc / 100f);

        public string kodQR { get; private set; }
        public string idKuriera { get; set; } // Kto aktualnie wiezie paczkę

        public Przesylka(float waga, Wymiary wymiar, bool czyDelikatna, string adres = "", string status = "Oczekiwanie na")
        {
            idPrzesylki = index++;
            this.waga = waga;
            this.szerokosc = wymiar.szerokosc;
            this.wysokosc = wymiar.wysokosc;
            this.dlugosc = wymiar.dlugosc;
            this.czyDelikatna = czyDelikatna;
            this.status = status;
            this.adres = adres;


            this.kodQR = $"PI-2026-{idPrzesylki.ToString().PadLeft(6, '0')}"; // Format z neta
        }
    }

    public class Zwrot : IDataRekord
    {
        static private int index = 1;

        public Zwrot(string przyczyna)
        {
            data = DateTime.Now;
            idZwrotu = index++;
            this.przyczyna = przyczyna;
            przesylki = new List<Przesylka>(); // POPRAWKA: Inicjalizacja listy
        }

        public int idZwrotu { get; private set; }
        public DateTime data { get; private set; }
        public string przyczyna { get; private set; }
        public List<Przesylka> przesylki { get; private set; }
    }

    namespace Postin.Aplikacja
    {
        public class OdpowiedzTestu
        {
            public string STATUS { get; set; }
            public string ERROR_MESSAGE { get; set; }
        }
    }
}