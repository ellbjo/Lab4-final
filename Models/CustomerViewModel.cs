namespace Lab4.Models
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public List<BookInstance> CurrentBorrowedBooks { get; set; }
    }
}
