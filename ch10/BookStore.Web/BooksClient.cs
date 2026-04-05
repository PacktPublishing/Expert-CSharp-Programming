using BookStore.Core.Models;

namespace BookStore.Web;

public class BooksClient(HttpClient client)
{
    public Task<Book[]?> GetBooksAsync(string url)
    {
        return client.GetFromJsonAsync<Book[]>(url);
    }
}
