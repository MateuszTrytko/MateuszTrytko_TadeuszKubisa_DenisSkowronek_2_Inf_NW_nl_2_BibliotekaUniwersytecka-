using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaApp
{
    public class BibliotekaContext : DbContext
    {
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Ksiazka> Ksiazki { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=biblioteka.db");
        }
    }
}
