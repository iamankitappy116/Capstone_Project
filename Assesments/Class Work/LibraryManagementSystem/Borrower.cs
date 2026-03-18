using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public class Borrower
    {
        public string Name { get; }
        public string LibraryCardNumber { get; }
        public List<Book> BorrowedBooks { get; }

        public Borrower(string name, string libraryCardNumber)
        {
            Name = name;
            LibraryCardNumber = libraryCardNumber;
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
