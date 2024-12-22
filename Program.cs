using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// Klasa Ksiazka reprezentująca tabelę Ksiazki w bazie danych
public class Ksiazka
{
    public int Id { get; set; }
    public string Tytul { get; set; }
    public string Autor { get; set; }
    public string Gatunek { get; set; }
    public int NumerPolki { get; set; }
    public bool Dostepna { get; set; }
}

// Klasa Uzytkownik reprezentująca tabelę Uzytkownicy w bazie danych
public class Uzytkownik
{
    public int Id { get; set; }
    public string Imie { get; set; }
    public string Email { get; set; }
    public string Haslo { get; set; }
    public bool CzyAdministrator { get; set; }
}

// Klasa Rezerwacja reprezentująca tabelę Rezerwacje w bazie danych
public class Rezerwacja
{
    public int Id { get; set; }
    public int KsiazkaId { get; set; }
    public Ksiazka Ksiazka { get; set; }
    public int UzytkownikId { get; set; }
    public Uzytkownik Uzytkownik { get; set; }
    public DateTime DataRezerwacji { get; set; }
    public DateTime TerminZwrotu { get; set; }
}

// Klasa DbContext, która zarządza połączeniem z bazą danych
public class BibliotekaContext : DbContext
{
    public DbSet<Ksiazka> Ksiazki { get; set; }
    public DbSet<Uzytkownik> Uzytkownicy { get; set; }
    public DbSet<Rezerwacja> Rezerwacje { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=biblioteka.db");
    }
}

public class AplikacjaBiblioteczna
{
    static void Main()
    {
        // Uruchomienie procesu logowania przy starcie aplikacji
        Zaloguj();
    }

    static void Zaloguj()
    {
        using (var context = new BibliotekaContext())
        {
            // Tworzy bazę danych, jeśli jeszcze nie istnieje
            context.Database.EnsureCreated();

            // Dodanie przykładowych danych do bazy, jeśli nie ma jeszcze użytkowników
            if (!context.Uzytkownicy.Any())
            {
                context.Uzytkownicy.Add(new Uzytkownik
                {
                    Imie = "Admin",
                    Email = "admin@biblioteka.com",
                    Haslo = "admin123",
                    CzyAdministrator = true
                });

                context.Uzytkownicy.Add(new Uzytkownik
                {
                    Imie = "Anna",
                    Email = "anna@przyklad.com",
                    Haslo = "user123",
                    CzyAdministrator = false
                });

                context.SaveChanges();
            }

            Console.WriteLine("Witaj w aplikacji bibliotecznej!\n");

            // Wprowadzenie logowania
            Console.Write("Podaj email: ");
            string email = Console.ReadLine();

            Console.Write("Podaj hasło: ");
            string haslo = Console.ReadLine();

            var uzytkownik = context.Uzytkownicy.FirstOrDefault(u => u.Email == email && u.Haslo == haslo);

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

        using (var context = new BibliotekaContext())
        {
            switch (opcja)
            {
                case 1:
                    var uzytkownicy = context.Uzytkownicy.ToList();
                    Console.WriteLine("\nLista użytkowników:");
                    foreach (var uzytkownik in uzytkownicy)
                    {
                        Console.WriteLine($"Id: {uzytkownik.Id}, Imię: {uzytkownik.Imie}, Email: {uzytkownik.Email}, Administrator: {uzytkownik.CzyAdministrator}");
                    }
                    break;
                case 2:
                    var ksiazki = context.Ksiazki.ToList();
                    Console.WriteLine("\nLista książek:");
                    foreach (var ksiazka in ksiazki)
                    {
                        Console.WriteLine($"Id: {ksiazka.Id}, Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}, Dostępna: {ksiazka.Dostepna}");
                    }
                    break;
                case 3:
                    var rezerwacje = context.Rezerwacje.Include(r => r.Ksiazka).Include(r => r.Uzytkownik).ToList();
                    Console.WriteLine("\nLista rezerwacji:");
                    foreach (var rezerwacja in rezerwacje)
                    {
                        Console.WriteLine($"Książka: {rezerwacja.Ksiazka.Tytul}, Użytkownik: {rezerwacja.Uzytkownik.Imie}, Termin zwrotu: {rezerwacja.TerminZwrotu}");
                    }
                    break;
                case 4:
                    Zaloguj();
                    return;
                default:
                    Console.WriteLine("Nieprawidłowa opcja.");
                    break;
            }
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

        using (var context = new BibliotekaContext())
        {
            switch (opcja)
            {
                case 1:
                    var mojeRezerwacje = context.Rezerwacje.Include(r => r.Ksiazka).Where(r => r.UzytkownikId == uzytkownik.Id).ToList();
                    Console.WriteLine("\nTwoje książki:");
                    foreach (var rezerwacja in mojeRezerwacje)
                    {
                        Console.WriteLine($"Tytuł: {rezerwacja.Ksiazka.Tytul}, Autor: {rezerwacja.Ksiazka.Autor}, Termin zwrotu: {rezerwacja.TerminZwrotu}");
                    }
                    break;
                case 2:
                    Console.Write("Podaj tytuł lub autora do wyszukania: ");
                    string wyszukiwanie = Console.ReadLine();
                    var znalezioneKsiazki = context.Ksiazki.Where(k => k.Tytul.Contains(wyszukiwanie) || k.Autor.Contains(wyszukiwanie)).ToList();
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
        }

        MenuUzytkownika(uzytkownik);
    }
}
