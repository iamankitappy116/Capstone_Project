using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public class Library
    {
        public List<Book> Books { get; }
        public List<Borrower> Borrowers { get; }

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

        public void BorrowBook(string isbn, string libraryCardNumber)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == isbn);
            var borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == libraryCardNumber);

            if (book == null || borrower == null)
                throw new InvalidOperationException("Book or Borrower not found.");

            borrower.BorrowBook(book);
        }

        public void ReturnBook(string isbn, string libraryCardNumber)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == isbn);
            var borrower = Borrowers.FirstOrDefault(b => b.LibraryCardNumber == libraryCardNumber);

            if (book == null || borrower == null)
                throw new InvalidOperationException("Book or Borrower not found.");

            borrower.ReturnBook(book);
        }

        public List<Book> ViewBooks() => Books;

        public List<Borrower> ViewBorrowers() => Borrowers;
    }
}
