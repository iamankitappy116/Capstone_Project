# BookStore RESTful API — ASP.NET Core 8

A fully RESTful API for managing **Books** and **Authors**, built with ASP.NET Core 8 and Entity Framework Core (SQLite).  
Covers all five user stories: CRUD endpoints, Postman testing, Fiddler debugging, association routing, and attribute routing.

---

## Quick Start

```bash
# 1. Navigate to the project folder
cd BookStoreAPI/BookStoreAPI

# 2. Restore NuGet packages
dotnet restore

# 3. Run the API (database + seed data are created automatically)
dotnet run

# 4. Open Swagger UI in your browser
#    http://localhost:5000/swagger   (or https://localhost:5001/swagger)
```

The SQLite database file (`bookstore.db`) is created automatically in the project directory on first run, pre-seeded with 3 authors and 5 books.

---

## Project Structure

```
BookStoreAPI/
├── Controllers/
│   ├── BooksController.cs       # CRUD for /api/books
│   └── AuthorsController.cs     # CRUD for /api/authors + nested /books route
├── Data/
│   └── BookStoreContext.cs      # EF Core DbContext + seed data
├── DTOs/
│   └── BookStoreDtos.cs         # Request/response shapes
├── Models/
│   ├── Book.cs                  # Book entity
│   └── Author.cs                # Author entity
├── Program.cs                   # App setup, DI, middleware
├── appsettings.json
└── BookStoreAPI.csproj
```

---

## API Endpoints

### Books  (`/api/books`)

| Method | Route              | Description                        | Success Code |
|--------|--------------------|------------------------------------|-------------|
| GET    | `/api/books`       | Get all books                      | 200 OK      |
| GET    | `/api/books/{id}`  | Get a book by ID                   | 200 OK      |
| POST   | `/api/books`       | Create a new book                  | 201 Created |
| PUT    | `/api/books/{id}`  | Update an existing book            | 200 OK      |
| DELETE | `/api/books/{id}`  | Delete a book                      | 204 No Content |

### Authors  (`/api/authors`)

| Method | Route                          | Description                        | Success Code |
|--------|--------------------------------|------------------------------------|-------------|
| GET    | `/api/authors`                 | Get all authors                    | 200 OK      |
| GET    | `/api/authors/{id}`            | Get an author by ID                | 200 OK      |
| POST   | `/api/authors`                 | Create a new author                | 201 Created |
| PUT    | `/api/authors/{id}`            | Update an existing author          | 200 OK      |
| DELETE | `/api/authors/{id}`            | Delete an author (cascades books)  | 204 No Content |
| GET    | `/api/authors/{authorId}/books`| Get all books by a specific author | 200 OK      |

---

## Request / Response Examples

### POST /api/books
```json
// Request body
{
  "title": "Brave New World",
  "publicationYear": 1932,
  "isbn": "9780060850524",
  "authorId": 1
}

// Response: 201 Created
{
  "id": 6,
  "title": "Brave New World",
  "publicationYear": 1932,
  "isbn": "9780060850524",
  "authorId": 1,
  "authorName": "George Orwell"
}
```

### PUT /api/books/1
```json
// Request body
{
  "title": "1984 (Updated Edition)",
  "publicationYear": 1949,
  "isbn": "9780451524935",
  "authorId": 1
}
```

### POST /api/authors
```json
// Request body
{
  "name": "Aldous Huxley",
  "bio": "English writer and philosopher, author of Brave New World."
}
```

---

## HTTP Status Codes

| Code | Meaning                                    |
|------|--------------------------------------------|
| 200  | OK — request succeeded                     |
| 201  | Created — new resource created             |
| 204  | No Content — deleted successfully          |
| 400  | Bad Request — validation failed / bad input|
| 404  | Not Found — resource doesn't exist         |

---

## Testing with Postman (User Story 2)

1. Download and install [Postman](https://www.postman.com/downloads/).
2. Start the API with `dotnet run`.
3. Create a new **Collection** called `BookStore API`.

### Step-by-step test sequence

**1. Get all books**
- Method: `GET`
- URL: `http://localhost:5000/api/books`
- Expected: `200 OK` with JSON array

**2. Get a specific book**
- Method: `GET`
- URL: `http://localhost:5000/api/books/1`
- Expected: `200 OK` with single book object

**3. Create a new book**
- Method: `POST`
- URL: `http://localhost:5000/api/books`
- Headers: `Content-Type: application/json`
- Body (raw JSON):
```json
{
  "title": "Brave New World",
  "publicationYear": 1932,
  "isbn": "9780060850524",
  "authorId": 1
}
```
- Expected: `201 Created`

**4. Update a book**
- Method: `PUT`
- URL: `http://localhost:5000/api/books/1`
- Headers: `Content-Type: application/json`
- Body:
```json
{
  "title": "Nineteen Eighty-Four",
  "publicationYear": 1949,
  "isbn": "9780451524935",
  "authorId": 1
}
```
- Expected: `200 OK`

**5. Delete a book**
- Method: `DELETE`
- URL: `http://localhost:5000/api/books/6`
- Expected: `204 No Content`

**6. Test 404 — book not found**
- Method: `GET`
- URL: `http://localhost:5000/api/books/9999`
- Expected: `404 Not Found`

**7. Test 400 — invalid data**
- Method: `POST`
- URL: `http://localhost:5000/api/books`
- Body: `{}` (empty)
- Expected: `400 Bad Request` with validation errors

**8. Books by author (nested route)**
- Method: `GET`
- URL: `http://localhost:5000/api/authors/2/books`
- Expected: `200 OK` with Harry Potter books

---

## Debugging with Fiddler (User Story 3)

1. Download and install [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) or [Fiddler Everywhere](https://www.telerik.com/fiddler).
2. Start Fiddler **before** making requests in Postman.
3. Fiddler acts as an HTTP proxy (default: `127.0.0.1:8888`).

### What to check in Fiddler

| Check | Where to Look |
|-------|--------------|
| Request method (GET/POST/PUT/DELETE) | **Inspectors → Headers** tab |
| Request body JSON | **Inspectors → JSON** or **Raw** tab |
| Response status code | **Result** column in session list |
| Response headers (`Content-Type: application/json`) | **Inspectors → Headers** (response side) |
| Response body | **Inspectors → JSON** tab (response side) |
| Round-trip time | **Timeline** tab |

### Troubleshooting with Fiddler

- **Unexpected 400**: Check the raw request body in the **Raw** tab for malformed JSON.
- **404 on valid ID**: Verify the URL path — IDs must be integers (e.g., `/api/books/1` not `/api/books/one`).
- **Missing `Content-Type` header**: POST/PUT requests without `Content-Type: application/json` will return 415 Unsupported Media Type.
- **HTTPS certificate errors**: In Fiddler, go to **Tools → Options → HTTPS → Decrypt HTTPS traffic** to inspect HTTPS traffic.

---

## Validation Rules

### Book
| Field           | Rule                              |
|-----------------|-----------------------------------|
| title           | Required, 1–200 characters        |
| publicationYear | Required, between 1000 and 2100   |
| isbn            | Optional, 10–13 characters        |
| authorId        | Required, must reference valid author |

### Author
| Field | Rule                              |
|-------|-----------------------------------|
| name  | Required, 2–100 characters        |
| bio   | Optional, max 500 characters      |

---

## Technologies

- **ASP.NET Core 8** — Web framework
- **Entity Framework Core 8** — ORM
- **SQLite** — Lightweight embedded database (no server needed)
- **Swashbuckle / Swagger** — Interactive API documentation
- **System.ComponentModel.DataAnnotations** — Model validation
