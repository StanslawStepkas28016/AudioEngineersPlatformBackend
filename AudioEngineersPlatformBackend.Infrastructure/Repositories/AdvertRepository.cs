using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Extensions;
using AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class AdvertRepository : IAdvertRepository
{
    private readonly AudioEngineersPlatformDbContext _context;

    public AdvertRepository(
        AudioEngineersPlatformDbContext context
    )
    {
        _context = context;
    }

    public async Task<bool> DoesUserHaveAnyActiveAdvertByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        return await
            _context
                .Adverts
                .AnyAsync
                (
                    exp => exp.IdUser == idUser && exp.AdvertLog.IsActive && !exp.AdvertLog.IsDeleted,
                    cancellationToken
                );
    }

    public async Task<AdvertCategory?> FindAdvertCategoryByNameAsync(
        string categoryName,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .AdvertCategories
            .FirstOrDefaultAsync(ac => ac.CategoryName == categoryName, cancellationToken);
    }

    public async Task AddAdvertAsync(
        Advert advert,
        CancellationToken cancellationToken
    )
    {
        await _context.Adverts.AddAsync(advert, cancellationToken);
    }

    public async Task AddAdvertLogAsync(
        AdvertLog advertLog,
        CancellationToken cancellationToken
    )
    {
        await _context.AdvertLogs.AddAsync(advertLog, cancellationToken);
    }

    public async Task<Advert?> FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync(
        Guid idUser,
        Guid idAdvert,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Adverts
            .Include(exp => exp.AdvertLog)
            .FirstOrDefaultAsync(exp => exp.IdUser == idUser && exp.IdAdvert == idAdvert, cancellationToken);
    }

    public async Task<bool> DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert(
        Guid idUserReviewer,
        Guid idAdvert,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Adverts
            .Include(exp => exp.Reviews)
            .AnyAsync(exp => exp.IdAdvert == idAdvert && exp.IdUser == idUserReviewer, cancellationToken);
    }

    public async Task<bool> DoesAdvertExistByIdAdvertAsync(
        Guid idAdvert,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Adverts
            .AnyAsync(exp => exp.IdAdvert == idAdvert, cancellationToken);
    }

    public async Task AddReviewLogAsync(
        ReviewLog reviewLog,
        CancellationToken cancellationToken
    )
    {
        await _context.ReviewLogs.AddAsync(reviewLog, cancellationToken);
    }

    public async Task AddReviewAsync(
        Review review,
        CancellationToken cancellationToken
    )
    {
        await _context.Reviews.AddAsync(review, cancellationToken);
    }

    public async Task<AdvertDetailsDto?> FindAdvertDetailsByIdAdvertAsync(
        Guid idAdvert,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Adverts
            .Where(a => a.IdAdvert == idAdvert && !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .Select
            (a => new AdvertDetailsDto
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
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedListDto<AdvertSummaryDto>> FindAdvertSummariesAsync(
        string? categoryFilterTerm,
        string? sortOrder,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        IQueryable<AdvertSummaryDto> queryable = _context
            .Adverts
            .Where(a => !a.AdvertLog.IsDeleted && a.AdvertLog.IsActive)
            .Select
            (a => new AdvertSummaryDto
                {
                    IdAdvert = a.IdAdvert,
                    Title = a.Title,
                    IdUser = a.IdUser,
                    UserFirstName = a.User.FirstName,
                    UserLastName = a.User.LastName,
                    DateCreated = a.AdvertLog.DateCreated,
                    Price = a.Price,
                    DescriptionShort = a.Description,
                    CategoryName = a.AdvertCategory.CategoryName,
                    CoverImageKey = a.CoverImageKey,
                    Description = a.Description
                }
            )
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoryFilterTerm))
        {
            queryable = queryable.Where(a => a.CategoryName == categoryFilterTerm);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            queryable = queryable.Where
            (a => EF
                .Functions.ToTsVector("english", a.Title + " " + a.Description)
                .Matches(searchTerm)
            );
        }

        switch (sortOrder)
        {
            case "date_asc":
                queryable = queryable.OrderBy(a => a.DateCreated);
                break;
            case "price_asc":
                queryable = queryable.OrderBy(a => a.Price);
                break;
            case "price_desc":
                queryable = queryable.OrderByDescending(a => a.Price);
                break;
            default:
                queryable = queryable.OrderByDescending(a => a.DateCreated);
                break;
        }

        return await PagedListDtoExtension.ToPagedListAsync(queryable, page, pageSize, cancellationToken);
    }

    public async Task<PagedListDto<ReviewDto>> FindAdvertReviewsAsync(
        Guid idAdvert,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var queryable = _context
            .Reviews
            .Where(r => r.IdAdvert == idAdvert)
            .Select
            (r =>
                new ReviewDto
                {
                    IdReview = r.IdReview,
                    ClientFirstName = r.User.FirstName,
                    ClientLastName = r.User.LastName,
                    Content = r.Content,
                    SatisfactionLevel = r.SatisfactionLevel,
                    DateCreated = r.ReviewLog.DateCreated
                }
            )
            .OrderByDescending(r => r.DateCreated)
            .AsQueryable();

        return await PagedListDtoExtension.ToPagedListAsync(queryable, page, pageSize, cancellationToken);
    }

    public async Task<Guid> FindIdAdvertByIdUser(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Adverts
            .Where(exp => exp.IdUser == idUser && exp.AdvertLog.IsActive && !exp.AdvertLog.IsDeleted)
            .Select(exp => exp.IdAdvert)
            .FirstOrDefaultAsync(cancellationToken);
    }
}