using System;
using System.Linq;

namespace BibliotekaApp
{
    public class BibliotekaApp
    {
        private readonly BibliotekaContext _context;

        public BibliotekaApp(BibliotekaContext context)
        {
            _context = context;
        }

        public void Start()
        {
            Console.WriteLine("Witaj w aplikacji bibliotecznej!");

            while (true)
            {
                Console.Write("Podaj email: ");
                var email = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                var haslo = Console.ReadLine();

                var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Email == email && u.Haslo == haslo);

                if (uzytkownik != null)
                {
                    switch (uzytkownik.Rola)
                    {
                        case Rola.Administrator:
                            MenuAdministratora(uzytkownik);
                            break;
                        case Rola.Pracownik:
                            MenuPracownika(uzytkownik);
                            break;
                        default:
                            MenuUzytkownika(uzytkownik);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowe dane logowania. Spróbuj ponownie.");
                }
            }
        }

        private void MenuAdministratora(Uzytkownik admin)
        {
            Console.WriteLine("\nMenu administratora:");
            Console.WriteLine("1. Wyświetl listę użytkowników");
            Console.WriteLine("2. Wyświetl listę książek");
            Console.WriteLine("3. Dodaj użytkownika");
            Console.WriteLine("4. Usuń użytkownika");
            Console.WriteLine("5. Dodaj książkę");
            Console.WriteLine("6. Usuń książkę");
            Console.WriteLine("7. Wyloguj");
            Console.Write("Wybierz opcję: ");

            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        WyswietlUzytkownikow();
                        break;
                    case 2:
                        WyswietlKsiazki();
                        break;
                    case 3:
                        DodajUzytkownika();
                        break;
                    case 4:
                        UsunUzytkownika();
                        break;
                    case 5:
                        DodajKsiazke();
                        break;
                    case 6:
                        UsunKsiazke();
                        break;
                    case 7:
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
        }

        private void DodajUzytkownika()
        {
            Console.Write("Podaj imię: ");
            var imie = Console.ReadLine();
            Console.Write("Podaj email: ");
            var email = Console.ReadLine();
            Console.Write("Podaj hasło: ");
            var haslo = Console.ReadLine();
            Console.Write("Podaj rolę (Administrator/Pracownik/Uzytkownik): ");
            var rola = Enum.TryParse<Rola>(Console.ReadLine(), out var parsedRola) ? parsedRola : Rola.Uzytkownik;

            _context.Uzytkownicy.Add(new Uzytkownik { Imie = imie, Email = email, Haslo = haslo, Rola = rola });
            _context.SaveChanges();

            Console.WriteLine("Dodano użytkownika.");
        }

        private void UsunUzytkownika()
        {
            Console.Write("Podaj email użytkownika do usunięcia: ");
            var email = Console.ReadLine();
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Email == email);

            if (uzytkownik != null)
            {
                _context.Uzytkownicy.Remove(uzytkownik);
                _context.SaveChanges();
                Console.WriteLine("Użytkownik został usunięty.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono użytkownika.");
            }
        }

        private void DodajKsiazke()
        {
            Console.Write("Podaj tytuł: ");
            var tytul = Console.ReadLine();
            Console.Write("Podaj autora: ");
            var autor = Console.ReadLine();
            Console.Write("Podaj lokalizację w bibliotece: ");
            var lokalizacja = Console.ReadLine();

            _context.Ksiazki.Add(new Ksiazka { Tytul = tytul, Autor = autor, Dostepna = true, Lokalizacja = lokalizacja });
            _context.SaveChanges();

            Console.WriteLine("Dodano książkę.");
        }

        private void UsunKsiazke()
        {
            Console.Write("Podaj tytuł książki do usunięcia: ");
            var tytul = Console.ReadLine();
            var ksiazka = _context.Ksiazki.FirstOrDefault(k => k.Tytul == tytul);

            if (ksiazka != null)
            {
                _context.Ksiazki.Remove(ksiazka);
                _context.SaveChanges();
                Console.WriteLine("Książka została usunięta.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono książki.");
            }
        }

        private void WyswietlUzytkownikow()
        {
            Console.WriteLine("\nLista użytkowników:");
            foreach (var uzytkownik in _context.Uzytkownicy)
            {
                Console.WriteLine($"Imię: {uzytkownik.Imie}, Email: {uzytkownik.Email}, Rola: {uzytkownik.Rola}");
            }
        }

        private void WyswietlKsiazki()
        {
            Console.WriteLine("\nLista książek:");
            foreach (var ksiazka in _context.Ksiazki)
            {
                Console.WriteLine($"Tytuł: {ksiazka.Tytul}, Autor: {ksiazka.Autor}, Lokalizacja: {ksiazka.Lokalizacja}, Dostępna: {ksiazka.Dostepna}");
            }
        }
        private void MenuUzytkownika(Uzytkownik uzytkownik)
        {
            Console.WriteLine($"\nWitaj, {uzytkownik.Imie}!");
            Console.WriteLine("Menu użytkownika:");
            Console.WriteLine("1. Wyświetl listę książek");
            Console.WriteLine("2. Wypożycz książkę");
            Console.WriteLine("3. Zwrot książki");
            Console.WriteLine("4. Wyloguj");
            Console.Write("Wybierz opcję: ");

            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        WyswietlKsiazki();
                        break;
                    case 2:
                        WypozyczKsiazke(uzytkownik);
                        break;
                    case 3:
                        ZwrocKsiazke(uzytkownik);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Podaj liczbę.");
            }
        }
    
    private void WypozyczKsiazke(Uzytkownik uzytkownik)
        {
            Console.Write("Podaj tytuł książki do wypożyczenia: ");
            var tytul = Console.ReadLine();

            var ksiazka = _context.Ksiazki.FirstOrDefault(k => k.Tytul == tytul && k.Dostepna);

            if (ksiazka != null)
            {
                ksiazka.Dostepna = false;
                Console.WriteLine($"Książka '{ksiazka.Tytul}' została wypożyczona.");
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Nie znaleziono dostępnej książki o podanym tytule.");
            }
        }

        private void ZwrocKsiazke(Uzytkownik uzytkownik)
        {
            Console.Write("Podaj tytuł książki do zwrotu: ");
            var tytul = Console.ReadLine();

            var ksiazka = _context.Ksiazki.FirstOrDefault(k => k.Tytul == tytul && !k.Dostepna);

            if (ksiazka != null)
            {
                ksiazka.Dostepna = true;
                Console.WriteLine($"Książka '{ksiazka.Tytul}' została zwrócona.");
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Nie znaleziono książki o podanym tytule lub książka jest już zwrócona.");
            }
        }
        private void MenuPracownika(Uzytkownik uzytkownik)
        {
            Console.WriteLine($"\nWitaj, {uzytkownik.Imie}! (Rola: Pracownik)");
            Console.WriteLine("Menu pracownika:");
            Console.WriteLine("1. Wyświetl listę książek");
            Console.WriteLine("2. Dodaj książkę");
            Console.WriteLine("3. Usuń książkę");
            Console.WriteLine("4. Wyloguj");
            Console.Write("Wybierz opcję: ");

            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        WyswietlKsiazki();
                        break;
                    case 2:
                        DodajKsiazke();
                        break;
                    case 3:
                        UsunKsiazke();
                        break;
                    case 4:
                        return; 
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Podaj liczbę.");
            }
        }
        
    }

}
