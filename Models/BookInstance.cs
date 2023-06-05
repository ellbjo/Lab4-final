using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class BookInstance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
