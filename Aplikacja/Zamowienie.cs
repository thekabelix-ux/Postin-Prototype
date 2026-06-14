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
    public class Platnosc : IDataRekord
    {
        static private int nextIndex = 0;

        public int index = 0;
        public DateTime data;
        public float zarobiona;
        public string idZamowienia;

        public Platnosc(float zarobione, string id)
        {
            index = nextIndex++;
            data = DateTime.Now;
            this.zarobiona = zarobione;
            idZamowienia = id;
        }

        public override string ToString()
        {
            string message = index.ToString() + ". " + data.ToString("d") + " Zysk z zamowienia o id " + idZamowienia + " : " + zarobiona.ToString() + "zł";  
            return message;
        }
    }
    public class Zamowienie : IDataRekord
    {
        static private int numer = 0;


        public Zamowienie(string adres)
        {
            dataZlozenia = DateTime.Now;
            przesylki = new List<Przesylka>();
            historia = new List<string>(); 
            status = "Oczekiwanie na";      
            adresDostawy = adres;
        }

        public Zamowienie(string adres, float symulowany_koszt_uslugi)
        {
            dataZlozenia = DateTime.Now;
            przesylki = new List<Przesylka>();
            historia = new List<string>();
            status = "Oczekiwanie na";
            adresDostawy = adres;
            this.idZamowienia = $"PI-2026-{numer++.ToString().PadLeft(6, '0')}"; // Format z neta
            Zewnetrzne.BazyDanych.AddToBase("PLATNOSCI", new Platnosc(symulowany_koszt_uslugi, idZamowienia));
        }


        public float wagaLaczna { get; set; }
        public DateTime dataZlozenia { get; private set; }
        public string adresDostawy { get; private set; }
        public string status { get; set; }
        public List<Przesylka> przesylki { get; private set; }
        public List<string> historia { get; private set; }
        public string szacowanyCzasDostawy { get; set; }
        public string idKuriera { get; set; } 
        public string idZamowienia { get; set; }
        
        public void DodajDoZamowienia(Przesylka p)
        {
            wagaLaczna += p.waga;
            przesylki.Add(p);

            p.zamowienie = this;
        }

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

        public Zamowienie zamowienie { get; set; }

        // Zwraca objętość w metrach sześciennych (cm -> m)
        public float ObjetoscM3 => (szerokosc / 100f) * (wysokosc / 100f) * (dlugosc / 100f);

        public string kodQR { get; private set; }
        public string idKuriera { get; set; } // Kto aktualnie wiezie paczkę

        public string Adres => zamowienie.adresDostawy;

        public Przesylka(float waga, Wymiary wymiar, bool czyDelikatna, string status = "Oczekiwanie na")
        {
            idPrzesylki = index++;
            this.waga = waga;
            this.szerokosc = wymiar.szerokosc;
            this.wysokosc = wymiar.wysokosc;
            this.dlugosc = wymiar.dlugosc;
            this.czyDelikatna = czyDelikatna;
            this.status = status;


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