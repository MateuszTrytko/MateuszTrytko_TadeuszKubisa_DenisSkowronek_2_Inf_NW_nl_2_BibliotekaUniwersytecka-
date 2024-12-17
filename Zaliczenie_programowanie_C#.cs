using System;
using System.Collections.Generic;
using System.Linq;

public class Ksiazka
{
    public int Id { get; set; }
    public string Tytul { get; set; }
    public string Autor { get; set; }
    public string Gatunek { get; set; }
    public int NumerPolki { get; set; }
    public bool Dostepna { get; set; }
}

public class Uzytkownik
{
    public int Id { get; set; }
    public string Imie { get; set; }
    public string Email { get; set; }
}

public class Pracownik : Uzytkownik
{
    // Dodatkowe właściwości dla pracowników (np. rola, uprawnienia)
}

public class Rezerwacja
{
    public Ksiazka Ksiazka { get; set; }
    public Uzytkownik Uzytkownik { get; set; }
    public DateTime DataRezerwacji { get; set; }
    public DateTime TerminZwrotu { get; set; }
}

public class Biblioteka
{
    private List<Ksiazka> ksiazki = new List<Ksiazka>();
    private List<Rezerwacja> rezerwacje = new List<Rezerwacja>();

    // Dodaje książkę do biblioteki
    public void DodajKsiazke(Ksiazka ksiazka)
    {
        ksiazki.Add(ksiazka);
    }

    // Wyszukuje książki po różnych kryteriach
    public List<Ksiazka> WyszukajKsiazki(string tytul = "", string autor = "", string gatunek = "", int? numerPolki = null)
    {
        return ksiazki.Where(k => 
            (string.IsNullOrEmpty(tytul) || k.Tytul.Contains(tytul)) &&
            (string.IsNullOrEmpty(autor) || k.Autor.Contains(autor)) &&
            (string.IsNullOrEmpty(gatunek) || k.Gatunek.Contains(gatunek)) &&
            (!numerPolki.HasValue || k.NumerPolki == numerPolki.Value)
        ).ToList();
    }

    // Zwraca liczbę książek w bibliotece
    public int LiczbaKsiazek()
    {
        return ksiazki.Count;
    }

    // Rezerwuje książkę dla użytkownika
    public void RezerwujKsiazke(Ksiazka ksiazka, Uzytkownik uzytkownik)
    {
        if (ksiazka.Dostepna)
        {
            var rezerwacja = new Rezerwacja
            {
                Ksiazka = ksiazka,
                Uzytkownik = uzytkownik,
                DataRezerwacji = DateTime.Now,
                TerminZwrotu = DateTime.Now.AddDays(14)
            };
            rezerwacje.Add(rezerwacja);
            ksiazka.Dostepna = false;
        }
        else
        {
            Console.WriteLine("Książka jest już zarezerwowana.");
        }
    }

    // Przedłuża termin zwrotu rezerwacji
    public void PrzedluzRezerwacje(Ksiazka ksiazka, Uzytkownik uzytkownik)
    {
        var rezerwacja = rezerwacje.FirstOrDefault(r => r.Ksiazka == ksiazka && r.Uzytkownik == uzytkownik);
        if (rezerwacja != null)
        {
            rezerwacja.TerminZwrotu = rezerwacja.TerminZwrotu.AddDays(7);
            Console.WriteLine($"Rezerwacja przedłużona. Nowy termin zwrotu: {rezerwacja.TerminZwrotu}");
        }
        else
        {
            Console.WriteLine("Nie znaleziono aktywnej rezerwacji.");
        }
    }

    // Zwraca książkę i nalicza ewentualne opłaty za spóźnienie
    public void ZwrocKsiazke(Ksiazka ksiazka, Uzytkownik uzytkownik)
    {
        var rezerwacja = rezerwacje.FirstOrDefault(r => r.Ksiazka == ksiazka && r.Uzytkownik == uzytkownik);
        if (rezerwacja != null)
        {
            var dniSpoznienia = (DateTime.Now - rezerwacja.TerminZwrotu).Days;
            if (dniSpoznienia > 0)
            {
                Console.WriteLine($"Spóźnienie o {dniSpoznienia} dni. Naliczona opłata.");
            }
            else
            {
                Console.WriteLine("Książka zwrócona na czas.");
            }
            rezerwacje.Remove(rezerwacja);
            ksiazka.Dostepna = true;
        }
        else
        {
            Console.WriteLine("Brak rezerwacji na tę książkę.");
        }
    }

    // Wyświetla najnowsze książki dodane do biblioteki
    public List<Ksiazka> PobierzNowosci(int liczba = 5)
    {
        return ksiazki.OrderByDescending(k => k.Id).Take(liczba).ToList();
    }
}

public class AplikacjaBiblioteczna
{
    static void Main()
    {
        var biblioteka = new Biblioteka();

        var ksiazka1 = new Ksiazka { Id = 1, Tytul = "Programowanie w C#", Autor = "Jan Kowalski", Gatunek = "Programowanie", NumerPolki = 5, Dostepna = true };
        var ksiazka2 = new Ksiazka { Id = 2, Tytul = "Zaawansowane C#", Autor = "Maria Nowak", Gatunek = "Programowanie", NumerPolki = 6, Dostepna = true };
        biblioteka.DodajKsiazke(ksiazka1);
        biblioteka.DodajKsiazke(ksiazka2);

        var uzytkownik = new Uzytkownik { Id = 1, Imie = "Anna", Email = "anna@przyklad.com" };

        Console.WriteLine("Wyszukiwanie książek po tytule:");
        var wynikiWyszukiwania = biblioteka.WyszukajKsiazki(tytul: "C#");
        foreach (var ksiazka in wynikiWyszukiwania)
        {
            Console.WriteLine($"Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}");
        }

        biblioteka.RezerwujKsiazke(ksiazka1, uzytkownik);
        Console.WriteLine("Nowości w bibliotece:");
        var nowosci = biblioteka.PobierzNowosci();
        foreach (var ksiazka in nowosci)
        {
            Console.WriteLine($"Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}");
        }

        biblioteka.PrzedluzRezerwacje(ksiazka1, uzytkownik);
        biblioteka.ZwrocKsiazke(ksiazka1, uzytkownik);
        Console.ReadKey();
    }
}
