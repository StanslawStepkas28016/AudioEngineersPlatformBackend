using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using AudioEngineersPlatformBackend.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class AdvertRepository : IAdvertRepository
{
    private readonly EngineersPlatformDbContext _context;

    public AdvertRepository(EngineersPlatformDbContext context)
    {
        _context = context;
    }


    public async Task<Advert?> GetActiveAndNonDeletedAdvertByIdUser(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdUser == idUser && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Advert?> GetActiveAndNonDeletedAdvertAndAdvertLogByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Include(a => a.AdvertLog)
            .Where(a => a.IdAdvert == idAdvert && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Advert?> GetActiveAndNonDeletedAdvertByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdAdvert == idAdvert && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AdvertDetailsDto?> GetActiveAndNonDeletedAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.User.IdUser == idUser && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
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

    public async Task<AdvertDetailsDto?> GetActiveAndNonDeletedAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdAdvert == idAdvert && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
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

    public async Task AddAdvert(Advert advert, CancellationToken cancellationToken)
    {
        await _context.Adverts.AddAsync(advert, cancellationToken);
    }

    public async Task AddAdvertLog(AdvertLog advertLog, CancellationToken cancellationToken)
    {
        await _context.AdvertLogs.AddAsync(advertLog, cancellationToken);
    }

    public async Task<PagedListDto<AdvertOverviewDto>> GetAllActiveAndNonDeletedAdvertsSummariesWithPagination(
        string? sortOrder, int page,
        int pageSize, string? searchTerm, CancellationToken cancellationToken)
    {
        IQueryable<AdvertOverviewDto> queryable = _context
            .Adverts
            .Where(a => !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .Select(a => new AdvertOverviewDto
            {
                IdAdvert = a.IdAdvert,
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
            queryable = queryable.Where(a => a.Description!.Contains(searchTerm.ToLower()));
        }

        switch (sortOrder)
        {
            case "date_desc":
                queryable = queryable.OrderByDescending(a => a.DateCreated);
                break;
            case "price_asc":
                queryable = queryable.OrderBy(a => a.Price);
                break;
            case "price_desc":
                queryable = queryable.OrderByDescending(a => a.Price);
                break;
            // "date_asc" is the default case
            default:
                queryable = queryable.OrderBy(a => a.DateCreated);
                break;
        }

        return await PagedListDtoExtension.ToPagedListAsync(queryable, page, pageSize, cancellationToken);
    }

    public async Task<Guid?> GetActiveAndNonDeletedIdAdvertByIdUser(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Adverts
            .Where(a => a.IdUser == idUser && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .Select(a => a.IdAdvert)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> FindReviewForAdvertByIdUserAndIdAdvert(Guid idAdvert, Guid idUser,
        CancellationToken cancellationToken)
    {
        return await _context
            .Reviews
            .Where(a => a.IdAdvert == idAdvert && a.IdUser == idUser)
            .Select(a => a.IdAdvert)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddReviewLog(ReviewLog reviewLog, CancellationToken cancellationToken)
    {
        await _context.ReviewLogs.AddAsync(reviewLog, cancellationToken);
    }

    public async Task AddReview(Review review, CancellationToken cancellationToken)
    {
        await _context.Reviews.AddAsync(review, cancellationToken);
    }

    public async Task<PagedListDto<ReviewDto>> GetReviewsForAdvertPaginated(Guid idAdvert, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var queryable = _context
            .Reviews
            .Select(r =>
                new ReviewDto(r.IdReview, r.User.FirstName, r.User.LastName, r.Content, r.SatisfactionLevel,
                    r.ReviewLog.DateCreated))
            .AsQueryable();

        return await PagedListDtoExtension.ToPagedListAsync(queryable, page, pageSize, cancellationToken);
    }
}