using Books.Services;

using Books.Data;

using System.Net.Http.Json;

namespace BlazorClient.Client.Services;

public class BooksClient(HttpClient httpClient, ILogger<BooksClient> logger) : IBooksService
{
    public async Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/books", book, cancellationToken);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<Book>(cancellationToken: cancellationToken);
            return created ?? throw new InvalidOperationException("API returned no book on create.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error creating book");
            throw;
        }
    }

    public async Task<int> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"/api/books/{id}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return 0;
            }
            response.EnsureSuccessStatusCode();
            return 1;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error deleting book {BookId}", id);
            throw;
        }
    }

    public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/books/{id}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error getting book {BookId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync("/api/books", cancellationToken);
            response.EnsureSuccessStatusCode();
            var books = await response.Content.ReadFromJsonAsync<IEnumerable<Book>>(cancellationToken: cancellationToken);
            return books ?? [];
        }
        catch (HttpRequestException ex) // thrown by EnsureSuccessStatusCode
        {
            logger.LogError(ex, "Error getting books");
            throw;
        }
    }

    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"/api/books/{book.Id}", book, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return 0;
            }
            response.EnsureSuccessStatusCode();
            return 1;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error updating book {BookId}", book.Id);
            throw;
        }
    }
}
