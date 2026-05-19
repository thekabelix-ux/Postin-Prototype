using System;

using Postin.Aplikacja;
using Postin.Pomocnicze;

namespace Postin
{
    class Program
    {
        static void Main(string[] args)
        {


            /// Pierwszy segment testów logowanie na różne typy kont wykorzystując różne dane ///
            /// wersja nie jest ostateczna muszę lekko przerobić system zalogowania się by wyszukiwał użytkowników z bazy a nie podawać klase
            Administrator a = new Administrator("Admin2", "1234", "A", "B", "C");
            Klient k = new Klient("Klient", "4321", "A", "B", "C", "@");

            string login = "Admin", haslo = "123";

            bool akcja_logowania()
            {
                return a.zaloguj(login, haslo);
            };

            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            login = "Admin2"; // Poprawa loginu na instniejący
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            haslo = "1234"; // Poprawa hasła
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");

            // Klienta dodam po zmianach w logowaniu

            //////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
