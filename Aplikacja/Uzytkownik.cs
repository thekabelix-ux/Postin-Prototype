using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Postin.Aplikacja
{
    internal static class NextIndex
    {
        internal static int ID_QUEUE = 1;
        internal static int GetNext()
        {
            return ID_QUEUE++;
        }
    }

    abstract public class Uzytkownik : Postin.Zewnetrzne.IDataRekord
    {
        // Dane bazowe
        public int ID { protected set; get; }
        public string login { protected set; get; }
        protected string haslo;

        public string imie { protected set; get; }
        public string nazwisko { protected set; get; }
        public string adres_zamieszkania { protected set; get; }

        // Elementy dotyczące sesji
        protected bool sesja_aktywna;
        //

        


        // Metody
        static public bool Zaloguj(string login, string haslo)
        {
            Uzytkownik user_data = (Uzytkownik)(Postin.Zewnetrzne.BazyDanych.FindInBase("Uzytkownicy", (a) => { return a is Uzytkownik u && u.login == login; }));
            if (user_data == null) throw new ArgumentException("Nie ma takiego użytkownika");
            if (user_data.sesja_aktywna) throw new Exception("Jesteś już zalogowany");
            else if (user_data.haslo != haslo) throw new ArgumentException("Błędne hasło");
            user_data.sesja_aktywna = true;
            return user_data.sesja_aktywna;
        }


        public bool jestZalogowany()
        {
            return sesja_aktywna;
        }


        // Konstruktor
        public Uzytkownik(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania)
        {
            this.ID = NextIndex.GetNext();
            this.login = login;
            this.haslo = haslo;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.adres_zamieszkania = adres_zamieszkania;
        }
    }

    public class Kurier : Uzytkownik
    {
        public string nr_rejestracyjny { protected set; get; }



        // Konstruktor
        public Kurier(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania, string nr_rejestracyjny) : base(login, haslo, imie, nazwisko, _adres_zamieszkania)
        {
            this.nr_rejestracyjny = nr_rejestracyjny;
        }


        void PobierzPlan()
        {

        }

        void PotwierdzZaladunek()
        {

        }

        void PotwierdzDoreczenie()
        {

        }


    }


    public class Administrator : Uzytkownik
    {



        // Konstruktor
        public Administrator(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania) : base(login, haslo, imie, nazwisko, _adres_zamieszkania) {}

        public List<Zamowienie> PobierzZamowienia()
        {
            var data = Postin.Zewnetrzne.BazyDanych.GetDataBase("Zamowienia");

            var zamowienia = new List<Zamowienie>();
            foreach(var rekord in data)
            {
                zamowienia.Add((Zamowienie)rekord);
            }

            return zamowienia;
        }

        public List<Zamowienie> GrupujZamowienia()
        {
            var zamowienia = PobierzZamowienia();

            // logika grupowania do zaimplementowania

            return zamowienia;
        }
    }


    public class Klient : Uzytkownik
    {
        public string nr_telefonu { protected set; get; }
        public string email { protected set; get; }

        // Konstruktor
        public Klient(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania, string email, string nr_tel = "") : base(login, haslo, imie, nazwisko, _adres_zamieszkania) {
            this.nr_telefonu = nr_tel;
            this.email = email;
        }


        void SledzZamowienie(int nrZamowienia)
        {
            Zamowienie zamowienie = (Zamowienie)Postin.Zewnetrzne.BazyDanych.FindInBase("Zamowienia", (a) => { return a is Zamowienie z && z.numer == nrZamowienia; });

            // logika śledzenia do zaimplementowania
        }

        void ZglosZwrot(string przyczyna, params string[] idPrzesylek)
        {
            var zwrot = new Zwrot(przyczyna);

            foreach(var id in idPrzesylek)
            {
                var przesylka = (Przesylka)Postin.Zewnetrzne.BazyDanych.FindInBase("Przesylki", (a) => { return a is Przesylka p && p.idPrzesylki == id; });
                zwrot.przesylki.Add(przesylka);
            }

            Postin.Zewnetrzne.BazyDanych.AddToBase("Zwroty", zwrot);
        }

        void ZglosUwagi()
        {
        }

        void PobierzPotwierdzenie()
        {

        }
    }


    public class Sprzedawca : Uzytkownik
    {
        public string nip { protected set; get; }

        // Konstruktor
        public Sprzedawca(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania, string nip) : base(login, haslo, imie, nazwisko, _adres_zamieszkania) {
            this.nip = nip;
        }

        public void PrzekazZamowienie(Postin.Zewnetrzne.IDataRekord dane_paczki)
        {
          var base_ = Postin.Zewnetrzne.BazyDanych.GetDataBase("Przesylki");
        }

    }


}
