using System;
using Microsoft.EntityFrameworkCore;
using M_Sinca_Teodora_Ioana_Lab2.Models;

namespace M_Sinca_Teodora_Ioana_Lab2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new M_Sinca_Teodora_Ioana_Lab2Context(serviceProvider.GetRequiredService<DbContextOptions<M_Sinca_Teodora_Ioana_Lab2Context>>()))
            {
                // Verifică dacă există deja cărți în baza de date
                if (context.Book.Any())
                {
                    return; // BD a fost creată anterior
                }

                // Adăugare de cărți
                context.Book.AddRange(
                    new Book
                    {
                        Title = "Baltagul",
                        Price = Decimal.Parse("22")
                    },
                    new Book
                    {
                        Title = "Enigma Otiliei",
                        Price = Decimal.Parse("18")
                    },
                    new Book
                    {
                        Title = "Maitreyi",
                        Price = Decimal.Parse("27")
                    }
                );

                context.Author.AddRange(
                    new Author
                    {
                        FirstName = "Mihail",
                        LastName = "Sadoveanu"
                    },
                    new Author
                    {
                        FirstName = "George",
                        LastName = "Călinescu"
                    },
                    new Author
                    {
                        FirstName = "Mircea",
                        LastName = "Eliade"
                    }
                );

                context.Genre.AddRange(
                    new Genre { Name = "Roman" },
                    new Genre { Name = "Nuvelă" },
                    new Genre { Name = "Poezie" }
                );

               
                context.Customer.AddRange(
                    new Customer
                    {
                        Name = "Popescu Marcela",
                        Adress = "Str. Plopilor, nr. 24",
                        BirthDate = DateTime.Parse("1979-09-01")
                    },
                    new Customer
                    {
                        Name = "Mihăilescu Cornel",
                        Adress = "Str. București, nr. 45, ap. 2",
                        BirthDate = DateTime.Parse("1969-07-08")
                    }
                );

               
                context.SaveChanges();
            }
        }
    }
}
