using System;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BibliotekaContext())
            {
                context.Database.EnsureCreated();

                
                if (!context.Uzytkownicy.Any())
                {
                    SeedData.Seed(context);
                }

                var app = new BibliotekaApp(context);
                app.Start();
            }
        }
    }
}
