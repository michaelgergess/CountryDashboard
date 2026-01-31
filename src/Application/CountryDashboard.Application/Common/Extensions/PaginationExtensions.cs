using CountryDashboard.Application.Common.Enums;
using CountryDashboard.Application.Common.Models.Shared;
using System.Linq.Expressions;

namespace CountryDashboard.Application.Common.Extensions;

public static class PaginationExtensions
{
    private static IQueryable<T> ApplySorting<T>(this IQueryable<T> query,
                                                 Expression<Func<T, object>> sortExpression,
                                                 SortDirectionEnum sortDirection)
    {
        return sortDirection == SortDirectionEnum.Desc
            ? query.OrderByDescending(sortExpression)
            : query.OrderBy(sortExpression);
    }

    /// <summary>
    /// Asynchronously retrieves a single page of results from the specified query, applying pagination and optional
    /// sorting.
    /// </summary>
    /// <remarks>If sorting is requested, the method applies the specified sort field and direction using the
    /// provided sort expression factory. The method executes the query asynchronously and returns only the items for
    /// the requested page. The total count reflects the number of items in the original query before
    /// pagination.</remarks>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The source query to paginate and optionally sort. Must not be null.</param>
    /// <param name="paginationParams">The pagination parameters specifying the page number and page size. Must not be null.</param>
    /// <param name="sortBy">The field by which to sort the results. If set to SortByEnum.Non, no sorting is applied.</param>
    /// <param name="sortDirection">The direction in which to sort the results. If null, ascending order is used by default.</param>
    /// <param name="sortExpressionFactory">A function that returns a sorting expression for the specified sort field. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a PagedResult<T> with the items for
    /// the requested page, total item count, current page number, and page size.</returns>
    public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
                                                              PaginationParams paginationParams,
                                                              SortByEnum sortBy,
                                                              SortDirectionEnum? sortDirection,
                                                              Func<SortByEnum, Expression<Func<T, object>>> sortExpressionFactory)

    {

        var totalCount = await query.CountAsync();

        if (sortBy != SortByEnum.Non)
        {
            var direction = sortDirection ?? SortDirectionEnum.Asc;
            var sortExpression = sortExpressionFactory(sortBy);

            if (sortExpression != null)
                query = query.ApplySorting(sortExpression, direction);
        }

        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PagedResult<T>(items,
                                  totalCount,
                                  paginationParams.PageNumber,
                                  paginationParams.PageSize);
    }
}
