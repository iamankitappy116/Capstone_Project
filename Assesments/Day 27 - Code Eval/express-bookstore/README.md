# Express BookStore API

A simple REST API built using **Node.js and Express.js** demonstrating routing, middleware, query parameters, and CRUD operations.

---

## Features

- Express server setup
- Query parameters handling
- Custom middleware logging
- REST API for managing books
- Modular routing
- Error handling

---

## Project Structure

express-bookstore
│
├── server.js
├── README.md
│
├── middleware
│ └── logger.js
│
├── routes
│ └── books.js
│
└── package.json



---

## Installation

Clone the repository or download the project.

Install dependencies:


npm install


---

## Run the Server


node server.js


Server runs on:


http://localhost:4000


---

## API Endpoints

### Home Route


GET /


Response:


Welcome to Express Server


---

### Server Status


GET /status


Response:


{
"server": "running",
"uptime": "OK"
}


---

### Product Search


GET /products?name=Laptop


Response:


{
"query": "Laptop",
"message": "Searching for product: Laptop"
}


---

### Get All Books


GET /books


---

### Get Book by ID


GET /books/:id


Example


GET /books/1


---

### Add New Book


POST /books


Body:


{
"title": "Atomic Habits",
"author": "James Clear"
}


---

### Update Book


PUT /books/:id


---

### Delete Book


DELETE /books/:id


---

## Middleware

Custom logging middleware logs:

- HTTP Method
- URL
- Timestamp

Example:


[GET] /books - 2026-03-18T10:00:00Z


---

## Technologies Used

- Node.js
- Express.js

---
