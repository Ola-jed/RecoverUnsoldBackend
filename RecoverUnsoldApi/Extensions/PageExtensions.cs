using FluentPaginator.Lib.Page;

namespace RecoverUnsoldApi.Extensions;

public static class PageExtensions
{
    public static Page<TU> Map<T, TU>(this Page<T> page, Func<T, TU> mapper)
    {
        return new Page<TU>(
            page.Items.Select(mapper),
            page.PageNumber,
            page.PageSize,
            Total: page.Total,
            HasNext: page.HasNext
        );
    }
}