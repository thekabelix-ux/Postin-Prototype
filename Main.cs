using System;

using Postin.Aplikacja;
using Postin.Pomocnicze;

using Postin.Zewnetrzne;

namespace Postin
{
    class Program
    {
        static void Main(string[] args)
        {


            /// Pierwszy segment testów logowanie na różne typy kont wykorzystując różne dane ///
            /// wersja nie jest ostateczna muszę lekko przerobić system zalogowania się by wyszukiwał użytkowników z bazy a nie podawać klase
            /// 

            var data_base_name = "Uzytkownicy";
            BazyDanych.GetDataBase(data_base_name); // Tworzenie
            BazyDanych.AddToBase(data_base_name, new Administrator("Admin2", "1234", "A", "B", "C"));
            BazyDanych.AddToBase(data_base_name, new Klient("Klient", "4321", "A", "B", "C", "@"));

            string login = "Admin", haslo = "123";

            bool akcja_logowania() { return Uzytkownik.Zaloguj(login, haslo); };
            Console.WriteLine("Testy logowania - Administrator:");
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            login = "Admin2"; // Poprawa loginu na istniejący
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            haslo = "1234"; // Poprawa hasła
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            // próba logowania, gdy użytkownik jest już zalogowany
            Symulacja.hookAction<Exception>(akcja_logowania, "Logowanie zakończone suckcesem");

            // Klienta dodam po zmianach w logowaniu

            //////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
