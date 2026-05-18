using System;
using System.Collections.Generic;
using System.Linq;
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

    abstract public class Uzytkownik
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
        public bool zaloguj(string login, string haslo)
        {
            if (sesja_aktywna) throw new Exception("Jesteś już zalogowany");
            if (this.login != login) throw new ArgumentException("Błędny login");
            else if (this.haslo != haslo) throw new ArgumentException("Błędne hasło");
            sesja_aktywna = true;
            return sesja_aktywna;
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
    }


    public class Administrator : Uzytkownik
    {



        // Konstruktor
        public Administrator(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania) : base(login, haslo, imie, nazwisko, _adres_zamieszkania) {}
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
    }


    public class Sprzedawca : Uzytkownik
    {
        public string nip { protected set; get; }

        // Konstruktor
        public Sprzedawca(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania, string nip) : base(login, haslo, imie, nazwisko, _adres_zamieszkania) {
            this.nip = nip;
        }

    }


}
