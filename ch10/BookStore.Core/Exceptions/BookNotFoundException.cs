namespace BookStore.Core.Exceptions;

/// <summary>
/// Thrown when a requested book cannot be found in the store.
/// </summary>
public sealed class BookNotFoundException : Exception
{
    /// <summary>Gets the ID of the book that was not found.</summary>
    public int BookId { get; }

    public BookNotFoundException(int bookId)
        : base($"Book with ID {bookId} was not found.")
    {
        BookId = bookId;
    }

    public BookNotFoundException(int bookId, Exception innerException)
        : base($"Book with ID {bookId} was not found.", innerException)
    {
        BookId = bookId;
    }
}
