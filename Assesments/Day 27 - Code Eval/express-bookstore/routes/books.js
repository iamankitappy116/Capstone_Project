const express = require('express');

const router = express.Router();

// In-memory database
let books = [
    { id: 1, title: "1984", author: "Orwell" },
    { id: 2, title: "The Alchemist", author: "Coelho" }
];


// GET all books
router.get('/', (req, res) => {
    res.json(books);
});


// GET book by ID
router.get('/:id', (req, res) => {

    const id = parseInt(req.params.id);

    const book = books.find(b => b.id === id);

    if (!book) {
        return res.status(404).json({ message: "Book not found" });
    }

    res.json(book);
});


// POST new book
router.post('/', (req, res) => {

    const { title, author } = req.body;

    if (!title || !author) {
        return res.status(400).json({
            message: "Title and Author required"
        });
    }

    const newBook = {
        id: books.length + 1,
        title,
        author
    };

    books.push(newBook);

    res.status(201).json(newBook);
});


// PUT update book
router.put('/:id', (req, res) => {

    const id = parseInt(req.params.id);

    const { title, author } = req.body;

    const book = books.find(b => b.id === id);

    if (!book) {
        return res.status(404).json({
            message: "Book not found"
        });
    }

    book.title = title || book.title;
    book.author = author || book.author;

    res.json(book);
});


// DELETE book
router.delete('/:id', (req, res) => {

    const id = parseInt(req.params.id);

    books = books.filter(b => b.id !== id);

    res.json({
        message: "Book deleted successfully"
    });
});

module.exports = router;