const express = require('express');
const EventEmitter = require('events');
const bookService = require('./services/bookService');

const app = express();
const eventEmitter = new EventEmitter();
const PORT = process.env.PORT || 3000;

// Middleware to parse JSON
app.use(express.json());

// Event listeners for logging
eventEmitter.on('bookAdded', (book) => console.log(`[Event] Book Added: ${JSON.stringify(book)}`));
eventEmitter.on('bookUpdated', (book) => console.log(`[Event] Book Updated: ${JSON.stringify(book)}`));
eventEmitter.on('bookDeleted', (id) => console.log(`[Event] Book Deleted: ID ${id}`));

// Root route
app.get('/', (req, res) => {
    res.json({ message: "Welcome to Book Management API" });
});

// GET /books - Returns all books
app.get('/books', async (req, res) => {
    try {
        const books = await bookService.readBooks();
        res.json(books);
    } catch (error) {
        res.status(500).json({ error: 'Failed to fetch books' });
    }
});

// GET /books/:id - Returns a book by ID
app.get('/books/:id', async (req, res) => {
    try {
        const books = await bookService.readBooks();
        const book = books.find(b => b.id === parseInt(req.params.id));
        if (!book) {
            return res.status(404).json({ error: 'Book not found' });
        }
        res.json(book);
    } catch (error) {
        res.status(500).json({ error: 'Failed to fetch book' });
    }
});

// POST /books - Adds a new book
app.post('/books', async (req, res) => {
    try {
        const { title, author } = req.body;
        if (!title || !author) {
            return res.status(400).json({ error: 'Title and author are required' });
        }
        const books = await bookService.readBooks();
        const newBook = {
            id: books.length > 0 ? books[books.length - 1].id + 1 : 1,
            title,
            author
        };
        books.push(newBook);
        await bookService.writeBooks(books);
        eventEmitter.emit('bookAdded', newBook);
        res.status(201).json(newBook);
    } catch (error) {
        res.status(500).json({ error: 'Failed to add book' });
    }
});

// PUT /books/:id - Updates book details by ID
app.put('/books/:id', async (req, res) => {
    try {
        const { title, author } = req.body;
        const books = await bookService.readBooks();
        const index = books.findIndex(b => b.id === parseInt(req.params.id));
        if (index === -1) {
            return res.status(404).json({ error: 'Book not found' });
        }
        books[index] = { ...books[index], title: title || books[index].title, author: author || books[index].author };
        await bookService.writeBooks(books);
        eventEmitter.emit('bookUpdated', books[index]);
        res.json(books[index]);
    } catch (error) {
        res.status(500).json({ error: 'Failed to update book' });
    }
});

// DELETE /books/:id - Deletes a book by ID
app.delete('/books/:id', async (req, res) => {
    try {
        const id = parseInt(req.params.id);
        const books = await bookService.readBooks();
        const filteredBooks = books.filter(b => b.id !== id);
        if (books.length === filteredBooks.length) {
            return res.status(404).json({ error: 'Book not found' });
        }
        await bookService.writeBooks(filteredBooks);
        eventEmitter.emit('bookDeleted', id);
        res.status(204).send();
    } catch (error) {
        res.status(500).json({ error: 'Failed to delete book' });
    }
});

// Start server
app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
