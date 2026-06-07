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

            var data_base_name = "Uzytkownicy";
            BazyDanych.GetDataBase(data_base_name); // Tworzenie
            BazyDanych.AddToBase(data_base_name, new Administrator("Admin2", "1234", "A", "B", "C"));
            BazyDanych.AddToBase(data_base_name, new Klient("Klient", "4321", "A", "B", "C", "@"));

            string login = "Admin", haslo = "123";

            bool akcja_logowania() { return Uzytkownik.Zaloguj(login, haslo); };
            Symulacja.PrintInLine("\nTesty logowania - Administrator: \n", "Nie istniejący użytkownik\n");
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            Symulacja.MakeBreakLine();
            login = "Admin2"; // Poprawa loginu na istniejący
            Symulacja.PrintInLine("Zmiana loginu na istniejący");
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            Symulacja.MakeBreakLine();
            haslo = "1234"; // Poprawa hasła
            Symulacja.PrintInLine("Zmiana hasła na poprawne");
            Symulacja.hookAction<ArgumentException>(akcja_logowania, "Logowanie zakończone suckcesem");
            Symulacja.MakeBreakLine();
            // próba logowania, gdy użytkownik jest już zalogowany
            Symulacja.PrintInLine("Zalogowany użytkownik");
            Symulacja.hookAction<Exception>(akcja_logowania, "Logowanie zakończone suckcesem");
            Symulacja.MakeBreakLine();
            login = "Klient";
            haslo = "4321";
            Symulacja.PrintInLine("Testy logowania - Klient, a konkretnie tylko sprawdzenie czy jest możliwość logowania na inne profile");
            // Logowanie na klienta
            Symulacja.hookAction<Exception>(akcja_logowania, "Logowanie zakończone sukcesem");
            Symulacja.MakeBreakLine();

            // Klienta dodam po zmianach w logowaniu

            //////////////////////////////////////////////////////////////////////////////////////
            ///// Rejestrujemy poprawnego sprzedawcę w bazie uzytkowników
            Postin.Zewnetrzne.BazyDanych.AddToBase("Uzytkownicy", new Sprzedawca("UX12", "123", "Jan", "Kowalski", "Szczecin", "NIP1"));

            // Obiekt sprzedawcy obsługujący żądanie
            Sprzedawca procesor = new Sprzedawca("System", "System", "S", "S", "S", "S");

            string zz1 = "{\"Seller\": \"UX12\", \"Packages\": {\"1\":{\"SIZE\": {30,40,30}, \"WEIGHT\":40}}, \"TotalCost\": 100}"; Symulacja.MakeBreakLine();
            string zz2 = "{\"Seller\": \"UX12\", \"Packages\": {\"1\":{\"SIZE\": {30,40,30}, \"WEIGHT\":40}}, \"TotalCost\": -400}"; Symulacja.MakeBreakLine(); 
            string zz3 = "{\"Seller\": \"UX15\", \"Packages\": {\"1\":{\"SIZE\": {30,40,30}, \"WEIGHT\":40}}, \"TotalCost\": 100}"; Symulacja.MakeBreakLine();

            Symulacja.PrintInLine("Zestaw 1: " , procesor.PrzetworzDanePaczek(zz1));
            Symulacja.PrintInLine("Zestaw 2: " , procesor.PrzetworzDanePaczek(zz2));
            Symulacja.PrintInLine("Zestaw 3: " , procesor.PrzetworzDanePaczek(zz3));


            // Przygotowanie danych
            Administrator admin = new Administrator("admin", "1", "A", "A", "A");

            Zamowienie z1 = new Zamowienie("Szczecin, ul. Goplańska 17");
            Przesylka p1 = new Przesylka(5, new Wymiary(30, 20, 15), false, "Szczecin, ul. Goplańska 17");
            z1.przesylki.Add(p1);

            Zamowienie z2 = new Zamowienie("Szczecin, ul. Nysy 21");
            Przesylka p2 = new Przesylka(2, new Wymiary(20, 20, 10), false, "Szczecin, ul. Nysy 21");
            z2.przesylki.Add(p2);

            Zamowienie z3 = new Zamowienie("Szczecin, ul. Malborska 9");
            Przesylka p3 = new Przesylka(7, new Wymiary(40, 30, 20), false, "Szczecin, ul. Malborska 9");
            z3.przesylki.Add(p3);

            //  Wrzucamy ZAMÓWIENIA do bazy danych
            BazyDanych.AddToBase("Zamowienia", z1);
            BazyDanych.AddToBase("Zamowienia", z2);
            BazyDanych.AddToBase("Zamowienia", z3);
            // Uruchomienie grupowania
            var wygenerowaneTrasy = admin.GrupujZamowieniaAutomatycznie();

            Console.WriteLine($"Liczba utworzonych tras: {wygenerowaneTrasy.Count}"); 

            for (int i = 0; i < wygenerowaneTrasy.Count; i++)
            {
                Console.WriteLine($"Trasa {i + 1} zawiera paczki:");
                foreach (var p in wygenerowaneTrasy[i])
                {
                    Console.WriteLine($" - Paczka do: {p.adres_dostawy} ({p.waga_laczna}kg)");
                }
            }

            //  Próba zatwierdzenia przez Admina
            bool wynikZatwierdzenia = admin.ZatwierdzPlanDostawy(wygenerowaneTrasy);
            Console.WriteLine($"Status zatwierdzenia planu: {wynikZatwierdzenia}"); // True


            // Tworzymy kuriera K01
            Kurier kurier = new Kurier("kurier1", "123", "Piotr", "Dostawca", "Szczecin", "K01");

            // Tworzymy paczkę testową (Ustawiamy jej ID na 1042 przez refleksję na polu index, żeby wymusić numer z testu) wsparłem się AI
            typeof(Przesylka).GetField("index", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)?.SetValue(null, 1042);

            Przesylka paczkaTestowa = new Przesylka(5, new Wymiary(30, 20, 15), false, "Szczecin, ul. Goplańska 17", "w trasie");
            paczkaTestowa.idKuriera = "K01"; // Przypisujemy kurierowi K01

            // Wrzucamy do bazy
            BazyDanych.AddToBase("Przesylki", paczkaTestowa);

            Console.WriteLine($"Wygenerowany kod QR paczki: {paczkaTestowa.kodQR}"); // Powinno być PI-2026-001042
            Console.WriteLine($"Status przed skanem: {paczkaTestowa.status}");

            // Kurier skanuje kod uzyskany z paczki
            string trescZeskanowana = "PI-2026-001042";
            bool wynikSkanowania = kurier.SkanujKodQR(trescZeskanowana);

            // Weryfikacja wyniku
            Console.WriteLine($"Czy skanowanie udane: {wynikSkanowania}"); // True
            Console.WriteLine($"Status po skanie: {paczkaTestowa.status}"); // dostarczona



            Klient klient = new Klient("user1", "pass", "Mikołaj", "K", "Szczecin", "m@zut.pl");

            // Tworzymy całe zamówienie o konkretnym kodzie QR
            Zamowienie zamowienieSledzone = new Zamowienie("Szczecin, ul. Goplańska 17");
            zamowienieSledzone.kodQR = "PI-2026-001042";
            zamowienieSledzone.status = "w dostawie";
            zamowienieSledzone.szacowanyCzasDostawy = "10.05.2026, godz. 9:00–13:00";

            // Dodajemy historię do zamówienia
            zamowienieSledzone.DodajKrokHistorii("zarejestrowano 08.05");
            zamowienieSledzone.DodajKrokHistorii("w terminalu 09.05");
            zamowienieSledzone.DodajKrokHistorii("w dostawie 10.05 08:45");

            // Dorzucamy paczkę gabarytową do tego zamówienia
            Przesylka paczka = new Przesylka(5, new Wymiary(30, 20, 15), false, "Szczecin, ul. Goplańska 17");
            zamowienieSledzone.przesylki.Add(paczka);

            // Wrzucamy ZAMÓWIENIE do bazy danych
            BazyDanych.AddToBase("Zamowienia", zamowienieSledzone);

            Console.WriteLine("=== TEST KLIENTA: ZESTAW 1 ===");
            string wynikZestaw1 = klient.SledzZamowienie("PI-2026-001042");
            Console.WriteLine(wynikZestaw1);

            Console.WriteLine("=== TEST KLIENTA: ZESTAW 2 ===");
            string wynikZestaw2 = klient.SledzZamowienie("PI-0000-000000");
            Console.WriteLine(wynikZestaw2);
        }
    }
}
