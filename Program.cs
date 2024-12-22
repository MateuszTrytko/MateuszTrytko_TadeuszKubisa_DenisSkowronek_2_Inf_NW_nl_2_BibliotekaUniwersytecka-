using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotekaApp
{
    class Program
    {
        static List<Uzytkownik> uzytkownicy = new List<Uzytkownik>();
        static List<Ksiazka> ksiazki = new List<Ksiazka>();
        static List<Rezerwacja> rezerwacje = new List<Rezerwacja>();

        static void Main(string[] args)
        {
            // Dodanie przykładowych danych
            DodajPrzykladoweDane();

            Zaloguj();
        }

        static void DodajPrzykladoweDane()
        {
            // Przykładowi użytkownicy
            uzytkownicy.Add(new Uzytkownik
            {
                Imie = "Admin",
                Email = "admin@biblioteka.com",
                Haslo = "admin123",
                CzyAdministrator = true
            });

            uzytkownicy.Add(new Uzytkownik
            {
                Imie = "Tomasz",
                Email = "TomaszHada@przyklad.com",
                Haslo = "user123",
                CzyAdministrator = false
            });

            // Przykładowe książki
            ksiazki.Add(new Ksiazka
            {
                Id = 1,
                Tytul = "Wiedźmin",
                Autor = "Andrzej Sapkowski",
                Dostepna = true
            });

            ksiazki.Add(new Ksiazka
            {
                Id = 2,
                Tytul = "Harry Potter",
                Autor = "J.K. Rowling",
                Dostepna = true
            });

            // Przykładowe rezerwacje
            rezerwacje.Add(new Rezerwacja
            {
                Id = 1,
                UzytkownikId = 2,
                KsiazkaId = 1,
                TerminZwrotu = DateTime.Now.AddDays(14)
            });
        }

        static void Zaloguj()
        {
            Console.WriteLine("Witaj w aplikacji bibliotecznej!\n");

            // Wprowadzenie logowania
            Console.Write("Podaj email: ");
            string email = Console.ReadLine();

            Console.Write("Podaj hasło: ");
            string haslo = Console.ReadLine();

            var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Email == email && u.Haslo == haslo);

            if (uzytkownik != null)
            {
                Console.WriteLine($"\nWitaj, {uzytkownik.Imie}!");

                // Przekierowanie na odpowiednie menu w zależności od typu użytkownika
                if (uzytkownik.CzyAdministrator)
                {
                    MenuAdministratora(uzytkownik);
                }
                else
                {
                    MenuUzytkownika(uzytkownik);
                }
            }
            else
            {
                Console.WriteLine("Błędny email lub hasło. Spróbuj ponownie.\n");
                Zaloguj();
            }
        }

        static void MenuAdministratora(Uzytkownik admin)
        {
            Console.WriteLine("\nMenu administratora:");
            Console.WriteLine("1. Wyświetl listę użytkowników");
            Console.WriteLine("2. Wyświetl listę książek");
            Console.WriteLine("3. Wyświetl listę rezerwacji");
            Console.WriteLine("4. Wyloguj");

            Console.Write("Wybierz opcję: ");
            int opcja = int.Parse(Console.ReadLine());

            switch (opcja)
            {
                case 1:
                    Console.WriteLine("\nLista użytkowników:");
                    foreach (var uzytkownik in uzytkownicy)
                    {
                        Console.WriteLine($"Imię: {uzytkownik.Imie}, Email: {uzytkownik.Email}, Administrator: {uzytkownik.CzyAdministrator}");
                    }
                    break;

                case 2:
                    Console.WriteLine("\nLista książek:");
                    foreach (var ksiazka in ksiazki)
                    {
                        Console.WriteLine($"Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}, Dostępna: {ksiazka.Dostepna}");
                    }
                    break;

                case 3:
                    Console.WriteLine("\nLista rezerwacji:");
                    foreach (var rezerwacja in rezerwacje)
                    {
                        var ksiazka = ksiazki.FirstOrDefault(k => k.Id == rezerwacja.KsiazkaId);
                        var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == rezerwacja.UzytkownikId);
                        Console.WriteLine($"Książka: {ksiazka.Tytul}, Użytkownik: {uzytkownik.Imie}, Termin zwrotu: {rezerwacja.TerminZwrotu}");
                    }
                    break;

                case 4:
                    Zaloguj();
                    return;

                default:
                    Console.WriteLine("Nieprawidłowa opcja.");
                    break;
            }

            MenuAdministratora(admin);
        }

        static void MenuUzytkownika(Uzytkownik uzytkownik)
        {
            Console.WriteLine("\nMenu użytkownika:");
            Console.WriteLine("1. Wyświetl moje książki");
            Console.WriteLine("2. Wyszukaj książki");
            Console.WriteLine("3. Wyloguj");

            Console.Write("Wybierz opcję: ");
            int opcja = int.Parse(Console.ReadLine());

            switch (opcja)
            {
                case 1:
                    Console.WriteLine("\nTwoje rezerwacje:");
                    var mojeRezerwacje = rezerwacje.Where(r => r.UzytkownikId == uzytkownik.Id).ToList();
                    foreach (var rezerwacja in mojeRezerwacje)
                    {
                        var ksiazka = ksiazki.FirstOrDefault(k => k.Id == rezerwacja.KsiazkaId);
                        Console.WriteLine($"Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}, Termin zwrotu: {rezerwacja.TerminZwrotu}");
                    }
                    break;

                case 2:
                    Console.Write("Podaj tytuł lub autora do wyszukania: ");
                    string wyszukiwanie = Console.ReadLine();
                    var znalezioneKsiazki = ksiazki.Where(k => k.Tytul.Contains(wyszukiwanie) || k.Autor.Contains(wyszukiwanie)).ToList();
                    Console.WriteLine("\nZnalezione książki:");
                    foreach (var ksiazka in znalezioneKsiazki)
                    {
                        Console.WriteLine($"Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}, Dostępna: {ksiazka.Dostepna}");
                    }
                    break;

                case 3:
                    Zaloguj();
                    return;

                default:
                    Console.WriteLine("Nieprawidłowa opcja.");
                    break;
            }

            MenuUzytkownika(uzytkownik);
        }
    }

    public class Uzytkownik
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Email { get; set; }
        public string Haslo { get; set; }
        public bool CzyAdministrator { get; set; }
    }

    public class Ksiazka
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public bool Dostepna { get; set; }
    }

    public class Rezerwacja
    {
        public int Id { get; set; }
        public int UzytkownikId { get; set; }
        public int KsiazkaId { get; set; }
        public DateTime TerminZwrotu { get; set; }
    }
}
