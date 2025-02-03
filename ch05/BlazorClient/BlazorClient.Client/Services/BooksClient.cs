using Books.Services;

using Books.Data;

using System.Net.Http.Json;

namespace BlazorClient.Client.Services;

public class BooksClient(HttpClient httpClient, ILogger<BooksClient> logger) : IBooksService
{
    public Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync("/apibooks", cancellationToken);
            response.EnsureSuccessStatusCode();
            var s = await response.Content.ReadAsStringAsync();
            var books = await response.Content.ReadFromJsonAsync<IEnumerable<Book>>(cancellationToken: cancellationToken);
            //var books = await httpClient.GetFromJsonAsync<IEnumerable<Book>>("books", cancellation);
            return books ?? [];
        }
        catch (HttpRequestException ex) // thrown by EnsureSuccessStatusCode
        {
            logger.LogError(ex, "Error getting books");
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
