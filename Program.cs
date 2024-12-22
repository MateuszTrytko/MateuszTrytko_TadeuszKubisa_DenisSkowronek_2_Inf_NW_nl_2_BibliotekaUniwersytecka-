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
        // Połączenie z lokalną bazą SQLite
        optionsBuilder.UseSqlite("Data Source=biblioteka.db");
    }
}

public class AplikacjaBiblioteczna
{
    static void Main()
    {
        using (var context = new BibliotekaContext())
        {
            // Tworzy bazę danych, jeśli jeszcze nie istnieje
            context.Database.EnsureCreated();

            // Dodanie przykładowych danych do bazy
            if (!context.Ksiazki.Any())
            {
                context.Ksiazki.Add(new Ksiazka
                {
                    Tytul = "Programowanie w C#",
                    Autor = "Jan Kowalski",
                    Gatunek = "Programowanie",
                    NumerPolki = 5,
                    Dostepna = true
                });

                context.Ksiazki.Add(new Ksiazka
                {
                    Tytul = "Zaawansowane C#",
                    Autor = "Maria Nowak",
                    Gatunek = "Programowanie",
                    NumerPolki = 6,
                    Dostepna = true
                });

                context.SaveChanges();
            }

            // Wyświetlenie wszystkich książek
            Console.WriteLine("Lista książek w bibliotece:");
            var ksiazki = context.Ksiazki.ToList();
            foreach (var ksiazka in ksiazki)
            {
                Console.WriteLine($"Id: {ksiazka.Id}, Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}");
            }

            // Dodanie użytkownika
            if (!context.Uzytkownicy.Any())
            {
                context.Uzytkownicy.Add(new Uzytkownik
                {
                    Imie = "Anna",
                    Email = "anna@przyklad.com"
                });
                context.SaveChanges();
            }

            // Rezerwacja książki
            var ksiazkaDoRezerwacji = context.Ksiazki.FirstOrDefault(k => k.Dostepna);
            var uzytkownik = context.Uzytkownicy.FirstOrDefault();

            if (ksiazkaDoRezerwacji != null && uzytkownik != null)
            {
                context.Rezerwacje.Add(new Rezerwacja
                {
                    KsiazkaId = ksiazkaDoRezerwacji.Id,
                    UzytkownikId = uzytkownik.Id,
                    DataRezerwacji = DateTime.Now,
                    TerminZwrotu = DateTime.Now.AddDays(14)
                });
                ksiazkaDoRezerwacji.Dostepna = false;
                context.SaveChanges();

                Console.WriteLine($"Zarezerwowano książkę: {ksiazkaDoRezerwacji.Tytul} dla użytkownika: {uzytkownik.Imie}");
            }
            else
            {
                Console.WriteLine("Brak dostępnych książek lub użytkowników do rezerwacji.");
            }

            // Wyświetlenie rezerwacji
            Console.WriteLine("\nLista rezerwacji:");
            var rezerwacje = context.Rezerwacje.Include(r => r.Ksiazka).Include(r => r.Uzytkownik).ToList();
            foreach (var rezerwacja in rezerwacje)
            {
                Console.WriteLine($"Książka: {rezerwacja.Ksiazka.Tytul}, Użytkownik: {rezerwacja.Uzytkownik.Imie}, Termin zwrotu: {rezerwacja.TerminZwrotu}");
            }
        }

        Console.ReadKey();
    }
}