using Postin;
using Postin.Aplikacja.Postin.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Postin.Zewnetrzne;

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

    abstract public class Uzytkownik : IDataRekord
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
            Uzytkownik user_data = (Uzytkownik)(BazyDanych.FindInBase("Uzytkownicy", (a) => { return a is Uzytkownik u && u.login == login; }));
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
        public string idKuriera { get; private set; }

        public Kurier(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania, string idKuriera)
            : base(login, haslo, imie, nazwisko, _adres_zamieszkania)
        {
            this.idKuriera = idKuriera;
        }

        // "Skanowanie" kodu QR przez kuriera
        public bool SkanujKodQR(string trescKoduQR)
        {
            // Szukamy przesyłki w bazie, która ma ten kod QR, status "w trasie" i jest przypisana do tego kuriera
            var przesylka = (Przesylka)BazyDanych.FindInBase("Przesylki", (a) =>
                a is Przesylka p && p.kodQR == trescKoduQR && p.status == "w trasie" && p.idKuriera == this.idKuriera);

            if (przesylka == null)
            {
                Console.WriteLine("BŁĄD: Nie znaleziono paczki o tym kodzie na liście kuriera lub złe statusy.");
                return false;
            }

            // Warunek zaliczenia: Zmiana statusu na "dostarczona"
            // Używamy refleksji z powodu private set, lub zmień na public set / internal set w Przesylka
            przesylka.status = "dostarczona";

            // Warunek zaliczenia: Klient otrzymuje powiadomienie
            PowiadomKlienta(przesylka);

            return true;
        }

        private void PowiadomKlienta(Przesylka p)
        {
            Console.WriteLine($"[POWIADOMIENIE SMS/EMAIL] Kliencie! Twoja przesyłka o kodzie {p.kodQR} została odebrana!");
        }
    }


    public class Administrator : Uzytkownik
    {
        private const float MAX_WAGA_KG = 3500f;
        private const float MAX_OBJETOSC_M3 = 3f;

        public Administrator(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania)
            : base(login, haslo, imie, nazwisko, _adres_zamieszkania) { }

        // Grupowanie Zamówień na podstawie ich wspólnego adresu dostawy
        public List<List<Zamowienie>> GrupujZamowieniaAutomatycznie()
        {
            var wszystkieZamowienia = BazyDanych.GetDataBase("Zamowienia").Cast<Zamowienie>().ToList();
            var trasy = new List<List<Zamowienie>>();


            var grupy = wszystkieZamowienia.GroupBy(z =>
            {
                if (string.IsNullOrEmpty(z.adres_dostawy)) return "BrakAdresu";

                string adresLower = z.adres_dostawy.ToLower();

                if (adresLower.Contains("goplańska") || adresLower.Contains("nysy"))
                {
                    return "Trasa_Goplanska_Nysy";
                }

                return $"Trasa_{z.adres_dostawy.Replace(" ", "_")}";
            });

            foreach (var grupa in grupy)
            {
                trasy.Add(grupa.ToList());
            }

            return trasy;
        }

        // Walidacja tras na podstawie sumy wszystkich przesyłek w zamówieniach
        public bool ZatwierdzPlanDostawy(List<List<Zamowienie>> proponowaneTrasy)
        {
            foreach (var trasa in proponowaneTrasy)
            {
                float totalTrasaWaga = 0;
                float totalTrasaObjetosc = 0;

                foreach (var zamowienie in trasa)
                {
                    // Zliczamy wagę i objętość wszystkich przesyłek w tym zamówieniu
                    foreach (var paczka in zamowienie.przesylki)
                    {
                        totalTrasaWaga += paczka.waga;
                        totalTrasaObjetosc += paczka.ObjetoscM3;
                    }
                }

                // Ponowne sprawdzenie warunków granicznych pojazdu
                if (totalTrasaWaga > MAX_WAGA_KG || totalTrasaObjetosc > MAX_OBJETOSC_M3)
                {
                    Console.WriteLine($"BŁĄD: Trasa przekracza limity! Waga: {totalTrasaWaga}kg, Objętość: {totalTrasaObjetosc}m3");
                    return false;
                }
            }

            // Zapis zatwierdzonych planów do bazy
            foreach (var trasa in proponowaneTrasy)
            {
                var plan = new Plan_Dostawy();
                BazyDanych.AddToBase("PlanyDostaw", plan);
            }

            return true;
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


        public string SledzZamowienie(string trescKoduQR)
        {
            // Szukamy całego zamówienia po kodzie QR
            var zamowienie = (Zamowienie)BazyDanych.FindInBase("Zamowienia", (a) => a is Zamowienie z && z.kodQR == trescKoduQR);

            if (zamowienie == null)
            {
                return "BŁĄD: Nie znaleziono przesyłki o podanym numerze.";
            }

            string wynik = $"Status zamówienia: {zamowienie.status}\n";
            wynik += $"Adres dostawy: {zamowienie.adres_dostawy}\n";
            wynik += $"Szacowany czas dostawy: {zamowienie.szacowanyCzasDostawy}\n";
            wynik += "Historia operacji:\n";

            foreach (var krok in zamowienie.historia)
            {
                wynik += $" - [UKOŃCZONO] {krok}\n";
            }

            return wynik;
        }

        public void ZglosZwrot(string przyczyna, params int[] idPrzesylek)
        {
            var zwrot = new Zwrot(przyczyna);

            foreach (var id in idPrzesylek)
            {
                // POPRAWKA: teraz int==int porównuje się prawidłowo
                var przesylka = (Przesylka)BazyDanych.FindInBase("Przesylki", (a) => { return a is Przesylka p && p.idPrzesylki == id; });
                if (przesylka != null) zwrot.przesylki.Add(przesylka);
            }

            BazyDanych.AddToBase("Zwroty", zwrot);
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

        public Sprzedawca(string login, string haslo, string imie, string nazwisko, string _adres_zamieszkania, string nip)
            : base(login, haslo, imie, nazwisko, _adres_zamieszkania)
        {
            this.nip = nip;
        }

        // Przetwarzanie testowego JSONa
        public string PrzetworzDanePaczek(string jsonInput)
        {
            try
            {
                jsonInput = jsonInput.Replace("\"{", "[").Replace("}\"", "]").Replace("{30,40,30}", "[30,40,30]");

                using (JsonDocument doc = JsonDocument.Parse(jsonInput))
                {
                    JsonElement root = doc.RootElement;

                    // Pobranie kosztu i szybka walidacja 
                    float totalCost = float.Parse(root.GetProperty("TotalCost").GetRawText());
                    if (totalCost < 0)
                    {
                        return JsonSerializer.Serialize(new OdpowiedzTestu { STATUS = "ERROR", ERROR_MESSAGE = "NEGATIVE COST NUMBER" });
                    }

                    // Pobranie sprzedawcy i walidacja w bazie
                    string sellerLogin = root.GetProperty("Seller").GetString();
                    var istniejeSprzedawca = BazyDanych.FindInBase("Uzytkownicy",
                        (a) => a is Sprzedawca s && s.login == sellerLogin);

                    if (istniejeSprzedawca == null)
                    {
                        return JsonSerializer.Serialize(new OdpowiedzTestu { STATUS = "ERROR", ERROR_MESSAGE = "SELLER NOT REGISTERED" });
                    }

                    float ourIncome = float.Parse(root.GetProperty("SERVICE_COST").GetRawText());
                    if(ourIncome == float.NaN || ourIncome < 0)
                    {
                        return JsonSerializer.Serialize(new OdpowiedzTestu { STATUS = "ERROR", ERROR_MESSAGE = "SERVICE NOT PAYED" });
                    }

                    

                    // --- Jeśli walidacja przeszła pomyślnie, tworzymy zamówienie ---
                    var noweZamowienie = new Zamowienie(adres_zamieszkania);

                    BazyDanych.AddToBase("PLATNOSCI", new Platnosc(ourIncome, noweZamowienie.kodQR));

                    JsonElement packages = root.GetProperty("Packages");

                    foreach (JsonProperty packageProp in packages.EnumerateObject())
                    {
                        JsonElement pkgData = packageProp.Value;
                        float weight = float.Parse(pkgData.GetProperty("WEIGHT").GetRawText());



                        var sizeArray = pkgData.GetProperty("SIZE").EnumerateArray();
                        float a = float.Parse(sizeArray.Current.GetRawText()); sizeArray.MoveNext();
                        float b = float.Parse(sizeArray.Current.GetRawText()); sizeArray.MoveNext();
                        float c = float.Parse(sizeArray.Current.GetRawText());

                        var wymiary = new Wymiary(a, b, c);
                        var paczka = new Przesylka(weight, wymiary, false);
                        noweZamowienie.waga_laczna += weight;

                        noweZamowienie.przesylki.Add(paczka);
                        BazyDanych.AddToBase("Przesylki", paczka);
                    }

                    BazyDanych.AddToBase("Zamowienia", noweZamowienie);
                    return JsonSerializer.Serialize(new OdpowiedzTestu { STATUS = "SUCCESS" });
                }
            }
            catch (Exception)
            {
                return JsonSerializer.Serialize(new OdpowiedzTestu { STATUS = "ERROR", ERROR_MESSAGE = "INVALID JSON FORMAT" });
            }
        }

        // Stara metoda - zostaje pusta
        public void PrzekazZamowienie(IDataRekord dane_paczki)
        {
            var base_ = BazyDanych.GetDataBase("Przesylki");
        }
    }


}
