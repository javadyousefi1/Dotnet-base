namespace Application.Common.Models;

public sealed record PaginatedResult<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);
