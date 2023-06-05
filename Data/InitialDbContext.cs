using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data
{
    public class InitialDbContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookInstance> BookInstances { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public InitialDbContext(DbContextOptions<InitialDbContext> options) : base(options) { }

    }
}
