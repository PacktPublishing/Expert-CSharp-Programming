# Exceptions bubbling up the stack

```mermaid
sequenceDiagram
    participant UI as UI/Client
    participant API as Books.API Endpoint
    participant SVC as BooksContext (IBooksService)
    participant EF as EF Core
    participant DB as SQLite

    UI->>API: GET /books
    API->>SVC: GetBooksAsync(ct)
    SVC->>EF: Books.ToListAsync(ct)
    EF->>DB: Execute SQL
    DB-->>EF: Error (e.g., constraint, IO)
    EF-->>SVC: throws Exception

    alt SqliteException
        SVC->>SVC: catch (SqliteException)\nwrap -> BookServiceException(HResult=3000)
        SVC-->>API: throws BookServiceException
    else Other exception
        Note over SVC: Exception filter LogErrorFilter(ex) runs\nlogs and returns false
        SVC--x API: Catch not taken (filter=false)
        SVC-->>API: exception bubbles up unchanged
    end

    API->>API: catch at boundary\n(map to ProblemDetails / 5xx)
    API-->>UI: Error response (ProblemDetails)
