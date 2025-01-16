namespace BibliotekaApp
{
    public class Uzytkownik
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Email { get; set; }
        public string Haslo { get; set; }
        public Rola Rola { get; set; }
    }

    public class Ksiazka
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public bool Dostepna { get; set; }
        public string Lokalizacja { get; set; }
    }

    public enum Rola
    {
        Administrator,
        Pracownik,
        Uzytkownik
    }
}
