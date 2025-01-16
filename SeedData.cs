using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibliotekaApp
{
    public static class SeedData
    {
        public static void Seed(BibliotekaContext context)
        {
            if (!context.Uzytkownicy.Any())
            {
                var uzytkownicy = OdczytajUzytkownikowZPliku("uzytkownicy.txt");
                context.Uzytkownicy.AddRange(uzytkownicy);
            }

            if (!context.Ksiazki.Any())
            {
                var ksiazki = OdczytajKsiazkiZPliku("ksiazki.txt");
                context.Ksiazki.AddRange(ksiazki);
            }

            context.SaveChanges();
        }

        private static List<Uzytkownik> OdczytajUzytkownikowZPliku(string sciezka)
        {
            var lista = new List<Uzytkownik>();
            foreach (var linia in File.ReadAllLines(sciezka))
            {
                var dane = linia.Split(',');
                lista.Add(new Uzytkownik
                {
                    Imie = dane[0],
                    Email = dane[1],
                    Haslo = dane[2],
                    Rola = Enum.TryParse<Rola>(dane[3], out var rola) ? rola : Rola.Uzytkownik
                });
            }
            return lista;
        }

        private static List<Ksiazka> OdczytajKsiazkiZPliku(string sciezka)
        {
            var lista = new List<Ksiazka>();
            foreach (var linia in File.ReadAllLines(sciezka))
            {
                var dane = linia.Split(',');
                lista.Add(new Ksiazka
                {
                    Tytul = dane[0],
                    Autor = dane[1],
                    Dostepna = bool.Parse(dane[2]),
                    Lokalizacja = dane[3]
                });
            }
            return lista;
        }
    }
}
