using AudioEngineersPlatformBackend.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Extensions;

public static class PagedListDtoExtension
{
    public static async Task<PagedListDto<T>> ToPagedListAsync<T>(
        IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        int totalCount = await query.CountAsync(cancellationToken);

        List<T> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedListDto<T>
        (
            items,
            page,
            pageSize,
            totalCount
        );
    }
}