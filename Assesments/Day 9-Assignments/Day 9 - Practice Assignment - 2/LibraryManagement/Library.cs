using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement
{
    public class Library
    {
        public List<Book> Books { get; private set; }
        public List<Borrower> Borrowers { get; private set; }

        public Library()
        {
            Books = new List<Book>();
            Borrowers = new List<Borrower>();
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public void RegisterBorrower(Borrower borrower)
        {
            Borrowers.Add(borrower);
        }

        public void BorrowBook(string isbn, string cardNumber)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == isbn);
            var borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == cardNumber);

            if (book == null || borrower == null)
                throw new Exception("Book or Borrower not found");

            borrower.BorrowBook(book);
        }

        public void ReturnBook(string isbn, string cardNumber)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == isbn);
            var borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == cardNumber);

            if (book == null || borrower == null)
                throw new Exception("Book or Borrower not found");

            borrower.ReturnBook(book);
        }

        public List<Book> ViewBooks()
        {
            return Books;
        }

        public List<Borrower> ViewBorrowers()
        {
            return Borrowers;
        }
    }
}