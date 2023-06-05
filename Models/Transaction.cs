using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public enum TransactionStatus
    {
        Borrowed,
        Returned
    }

    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int BookInstanceId { get; set; }
        public BookInstance BookInstance { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
