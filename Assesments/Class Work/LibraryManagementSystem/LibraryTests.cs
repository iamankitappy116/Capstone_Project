using Xunit;
using LibraryManagementSystem;
using System.Linq;

namespace LibraryManagementSystem.Tests
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
            var borrower = new Borrower("Ankit", "L001");

            library.RegisterBorrower(borrower);

            Assert.Single(library.Borrowers);
        }

        [Fact]
        public void BorrowBook_ShouldMarkBookAsBorrowed()
        {
            var library = new Library();
            var book = new Book("C# Basics", "John", "123");
            var borrower = new Borrower("Ankit", "L001");

            library.AddBook(book);
            library.RegisterBorrower(borrower);

            library.BorrowBook("123", "L001");

            Assert.True(book.IsBorrowed);
            Assert.Single(borrower.BorrowedBooks);
        }

        [Fact]
        public void ReturnBook_ShouldMarkBookAsAvailable()
        {
            var library = new Library();
            var book = new Book("C# Basics", "John", "123");
            var borrower = new Borrower("Ankit", "L001");

            library.AddBook(book);
            library.RegisterBorrower(borrower);

            library.BorrowBook("123", "L001");
            library.ReturnBook("123", "L001");

            Assert.False(book.IsBorrowed);
            Assert.Empty(borrower.BorrowedBooks);
        }

        [Fact]
        public void ViewBooksAndBorrowers_ShouldReturnCorrectLists()
        {
            var library = new Library();
            library.AddBook(new Book("Book1", "Author1", "111"));
            library.RegisterBorrower(new Borrower("User1", "L002"));

            Assert.Equal(1, library.ViewBooks().Count);
            Assert.Equal(1, library.ViewBorrowers().Count);
        }
    }
}