const express = require('express');
const bookRouter = require('./routes/books');
const logger = require('./middleware/logger');

const app = express();
const PORT = 4000;

// middleware to parse JSON
app.use(express.json());

// custom logging middleware
app.use(logger);

// Home route
app.get('/', (req, res) => {
    res.send("Welcome to Express Server");
});

// Status route
app.get('/status', (req, res) => {
    res.json({
        server: "running",
        uptime: "OK"
    });
});

// Query parameter example
app.get('/products', (req, res) => {
    const name = req.query.name;

    if (name) {
        res.json({
            query: name,
            message: `Searching for product: ${name}`
        });
    } else {
        res.send("Please provide a product name");
    }
});

// Modular route
app.use('/books', bookRouter);

// 404 handler
app.use((req, res) => {
    res.status(404).json({
        error: "Route not found"
    });
});

// Global error handler
app.use((err, req, res, next) => {
    console.error(err.stack);

    res.status(500).json({
        error: "Internal Server Error"
    });
});

app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});