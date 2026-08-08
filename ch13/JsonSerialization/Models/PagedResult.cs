namespace JsonSerialization.Models;

/// <summary>Represents a book search result with paging metadata.</summary>
public record class PagedResult<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    public bool HasNextPage => PageNumber * PageSize < TotalCount;
}
