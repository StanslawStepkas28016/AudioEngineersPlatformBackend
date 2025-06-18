using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
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
            .FirstOrDefaultAsync(a => a.IdUser == idUser, cancellationToken);
    }

    public async Task<AdvertAssociatedData?> GetAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdAdvert == idAdvert)
            .Select(a => new AdvertAssociatedData
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
}