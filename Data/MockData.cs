using Lab4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Lab4.Data
{
    public static class MockData
    {
        public static async void CreateMockData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<InitialDbContext>();

                context.Database.EnsureCreated();

                if (!context.Customers.Any())
                {
                    var customerList = new Customer[]
                    {
                    new Customer { Name = "Olle", Surname = "Svensson", PersNr = "9012110101", Email = "olle@gmail.com"},
                    new Customer { Name = "Ulla", Surname = "Olsson", PersNr = "9908030202", Email = "ulla@gmail.com" },
                    new Customer { Name = "Berra", Surname = "Bengtsson", PersNr = "6602031818", Email = "berra@gmail.com" },
                    };

                    context.Customers.AddRange(customerList);
                    context.SaveChanges();
                }

                if (!context.Books.Any())
                {
                    var bookList = new Book[]
                    {
                    new Book { Title = "Harry Potter", Description = "A great Wizard making big adventures" },
                    new Book { Title = "Lion King", Description = "A great Lion making big adventures" },
                    new Book { Title = "Lord of the rings", Description = "A great Hobbit making big adventures"},
                    new Book { Title = "Marry Poppins", Description = "A great Nanny making big adventures" },
                    new Book { Title = "Batman", Description = "A great Batman making big adventures" },
                    };

                    context.Books.AddRange(bookList);
                    context.SaveChanges();
                }

                if (!context.BookInstances.Any())
                {
                    var book = await context.Books.FirstAsync(b => b.Title == "Harry Potter");
                    var customer = await context.Customers.FirstAsync();

                    var bookInstanceList = new BookInstance[]
                    {
                        new BookInstance { Book = book },
                        new BookInstance { Book = book }
                    };

                    await context.BookInstances.AddRangeAsync(bookInstanceList);
                    await context.SaveChangesAsync();

                    var transactionList = new Transaction[]
                    {
                        new Transaction { Customer = customer, BookInstance = bookInstanceList[0], Status = TransactionStatus.Borrowed },
                        new Transaction { Customer = customer, BookInstance = bookInstanceList[1], Status = TransactionStatus.Returned }
                    };

                    await context.Transactions.AddRangeAsync(transactionList);
                    await context.SaveChangesAsync();

                }
            }

        }
    }
}