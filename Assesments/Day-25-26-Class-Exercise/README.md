# Book Management REST API

A simple Node.js REST API to manage a collection of books, using Express.js and JSON file storage.

## Features
- CRUD operations for books.
- Persistent storage in `data/books.json`.
- Event logging for data changes.
- Asynchronous file I/O with `fs/promises`.

## Project Structure
```
book-api/
├── data/
│   └── books.json
├── services/
│   └── bookService.js
├── server.js
├── package.json
└── README.md
```

## Setup Instructions

1. **Install dependencies:**
   ```bash
   npm install
   ```

2. **Run the server (using nodemon):**
   ```bash
   npm start
   ```
   The server will start on `http://localhost:3000`.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Welcome message |
| GET | `/books` | Get all books |
| GET | `/books/:id` | Get book by ID |
| POST | `/books` | Add a new book (Body: `{"title": "...", "author": "..."}`) |
| PUT | `/books/:id` | Update book details (Body: `{"title": "...", "author": "..."}`) |
| DELETE | `/books/:id` | Delete book by ID |

## Technologies Used
- Node.js
- Express.js
- Nodemon (Dev dependency)
- EventEmitter (Node.js built-in)
- fs/promises (Node.js built-in)

## PowerShell Usage Examples (Windows)

### 1. Welcome Message
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/" -Method Get
```

### 2. Add a New Book
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/books" -Method Post -Body '{"title": "The Great Gatsby", "author": "F. Scott Fitzgerald"}' -ContentType "application/json"
```

### 3. List All Books
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/books" -Method Get
```

### 4. Get Book by ID
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/books/1" -Method Get
```

### 5. Update Book Details
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/books/1" -Method Put -Body '{"title": "The Great Gatsby (Updated)"}' -ContentType "application/json"
```

### 6. Delete a Book
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/books/1" -Method Delete
```
