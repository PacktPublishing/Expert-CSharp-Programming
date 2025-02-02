namespace Books.Services;

public class BookServiceException : ApplicationException
{
	public BookServiceException() { }
	public BookServiceException(string message) : base(message) { }
	public BookServiceException(string message, Exception inner) : base(message, inner) { }
}
