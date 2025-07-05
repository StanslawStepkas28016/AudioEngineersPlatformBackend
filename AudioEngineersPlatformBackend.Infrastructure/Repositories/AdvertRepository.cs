using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using AudioEngineersPlatformBackend.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class AdvertRepository : IAdvertRepository
{
    private readonly EngineersPlatformDbContext _context;

    public AdvertRepository(EngineersPlatformDbContext context)
    {
        _context = context;
    }


    public async Task<Advert?> GetAdvertByIdUser(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdUser == idUser)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AdvertLog?> GetAdvertLogByIdAdvert(Guid idAdvert, CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdAdvert == idAdvert)
            .Select(a => a.AdvertLog)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Advert?> GetAdvertByIdAdvert(Guid idAdvert, CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdAdvert == idAdvert)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AdvertDetailsDto?> GetAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.User.IdUser == idUser)
            .Select(a => new AdvertDetailsDto
            {
                IdAdvert = a.IdAdvert,
                IdUser = a.IdUser,
                UserFirstName = a.User.FirstName,
                UserLastName = a.User.LastName,
                Title = a.Title,
                Description = a.Description,
                Price = a.Price,
                CategoryName = a.AdvertCategory.CategoryName,
                CoverImageKey = a.CoverImageKey,
                PortfolioUrl = a.PortfolioUrl,
                DateCreated = a.AdvertLog.DateCreated,
                DateModified = a.AdvertLog.DateModified
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AdvertCategory?> GetAdvertCategoryByCategoryName(string categoryName,
        CancellationToken cancellationToken)
    {
        return await _context
            .AdvertCategories
            .FirstOrDefaultAsync(ac => ac.CategoryName == categoryName, cancellationToken);
    }

    public async Task<Advert> AddAdvert(Advert advert, CancellationToken cancellationToken)
    {
        var entityEntry = await _context.Adverts.AddAsync(advert, cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<AdvertLog> AddAdvertLog(AdvertLog advertLog, CancellationToken cancellationToken)
    {
        var entityEntry = await _context.AdvertLogs.AddAsync(advertLog, cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<PagedListDto<AdvertOverviewDto>> GetAllAdvertsWithPagination(string? sortOrder, int page,
        int pageSize, string? searchTerm, CancellationToken cancellationToken)
    {
        var query = _context
            .Adverts
            .Select(a => new AdvertOverviewDto
            {
                IdAvert = a.IdAdvert,
                Title = a.Title,
                IdUser = a.IdUser,
                UserFirstName = a.User.FirstName,
                UserLastName = a.User.LastName,
                DateCreated = a.AdvertLog.DateCreated,
                Price = a.Price,
                CategoryName = a.AdvertCategory.CategoryName,
                CoverImageKey = a.CoverImageKey,
                Description = a.Description
            })
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(a => a.Description!.Contains(searchTerm.ToLower()));
        }

        switch (sortOrder)
        {
            case "date_desc":
                query = query.OrderByDescending(a => a.DateCreated);
                break;
            case "price_asc":
                query = query.OrderBy(a => a.Price);
                break;
            case "price_desc":
                query = query.OrderByDescending(a => a.Price);
                break;
            // "date_asc" is the default case
            default:
                query = query.OrderBy(a => a.DateCreated);
                break;
        }

        return await PagedListDtoExtension.ToPagedListAsync(query, page, pageSize, cancellationToken);
    }
}