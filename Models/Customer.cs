using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Surname { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(12)]
        public string PersNr { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public IEnumerable<BookInstance> GetCurrentBorrowedBooks()
        {
            var borrowedBooks = Transactions
                .Where(t => t.Status == TransactionStatus.Borrowed)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            var returnedBooks = Transactions
                .Where(t => t.Status == TransactionStatus.Returned)
                .ToList();

            foreach (var borrowedBook in borrowedBooks)
            {
                if (!returnedBooks.Any(rb => rb.BookInstance.Book.Title == borrowedBook.BookInstance.Book.Title && rb.TransactionDate > borrowedBook.TransactionDate))
                {
                    yield return borrowedBook.BookInstance;
                }
            }
        }
    }
}
