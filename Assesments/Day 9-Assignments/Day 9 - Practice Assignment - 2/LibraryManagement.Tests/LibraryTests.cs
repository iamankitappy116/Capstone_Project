using Xunit;
using LibraryManagement;
using System.Linq;

namespace LibraryManagement.Tests
{
    public class LibraryTests
    {
        [Fact]
        public void AddBook_ShouldAddBookToLibrary()
        {
            var library = new Library();
            var book = new Book("C# Basics", "John", "123");

            library.AddBook(book);

            Assert.Single(library.Books);
        }

        [Fact]
        public void RegisterBorrower_ShouldAddBorrower()
        {
            var library = new Library();
            var borrower = new Borrower("Ayan", "001");

            library.RegisterBorrower(borrower);

            Assert.Single(library.Borrowers);
        }

        [Fact]
        public void BorrowBook_ShouldMarkBookAsBorrowed()
        {
            var library = new Library();
            var book = new Book("C#", "John", "123");
            var borrower = new Borrower("Ayan", "001");

            library.AddBook(book);
            library.RegisterBorrower(borrower);

            library.BorrowBook("123", "001");

            Assert.True(book.IsBorrowed);
            Assert.Single(borrower.BorrowedBooks);
        }

        [Fact]
        public void ReturnBook_ShouldMarkBookAsAvailable()
        {
            var library = new Library();
            var book = new Book("C#", "John", "123");
            var borrower = new Borrower("Ayan", "001");

            library.AddBook(book);
            library.RegisterBorrower(borrower);

            library.BorrowBook("123", "001");
            library.ReturnBook("123", "001");

            Assert.False(book.IsBorrowed);
            Assert.Empty(borrower.BorrowedBooks);
        }

        [Fact]
        public void ViewBooks_ShouldReturnAllBooks()
        {
            var library = new Library();
            var book = new Book("C#", "John", "123");

            library.AddBook(book);

            var books = library.ViewBooks();

            Assert.Single(books);
        }

        [Fact]
        public void ViewBorrowers_ShouldReturnAllBorrowers()
        {
            var library = new Library();
            var borrower = new Borrower("Ayan", "001");

            library.RegisterBorrower(borrower);

            var borrowers = library.ViewBorrowers();

            Assert.Single(borrowers);
        }
    }
}