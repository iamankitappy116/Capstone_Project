using System.Collections.Generic;

namespace LibraryManagement
{
    public class Borrower
    {
        public string Name { get; set; }
        public string LibraryCardNumber { get; set; }
        public List<Book> BorrowedBooks { get; private set; }

        public Borrower(string name, string cardNumber)
        {
            Name = name;
            LibraryCardNumber = cardNumber;
            BorrowedBooks = new List<Book>();
        }

        public void BorrowBook(Book book)
        {
            book.Borrow();
            BorrowedBooks.Add(book);
        }

        public void ReturnBook(Book book)
        {
            book.Return();
            BorrowedBooks.Remove(book);
        }
    }
}