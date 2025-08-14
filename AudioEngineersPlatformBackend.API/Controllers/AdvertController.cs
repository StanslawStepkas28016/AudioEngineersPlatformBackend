using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert.AddReview;
using AudioEngineersPlatformBackend.Contracts.Advert.ChangeAdverData;
using AudioEngineersPlatformBackend.Contracts.Advert.CreateAdvert;
using AudioEngineersPlatformBackend.Contracts.Advert.GetAdvertDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/advert")]
public class AdvertController(IAdvertService advertService) : ControllerBase
{
    /// <summary>
    ///     Endpoint used for creating an advert.
    /// </summary>
    /// <param name="createAdvertRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPost]
    public async Task<IActionResult> CreateAdvert([FromForm] CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        CreateAdvertResponse createAdvertResponse =
            await advertService.CreateAdvert(createAdvertRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, createAdvertResponse);
    }

    /// <summary>
    ///     Endpoint used for changing data assigned to an advert with a specified idAdvert.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="changeAdvertDataRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPatch("{idAdvert:guid}")]
    public async Task<IActionResult> ChangeAdvertData(Guid idAdvert,
        [FromBody] ChangeAdvertDataRequest changeAdvertDataRequest,
        CancellationToken cancellationToken)
    {
        await advertService.ChangeAdvertData(idAdvert, changeAdvertDataRequest, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    ///     Endpoint used for soft-deleting an advert by its idAdvert.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpDelete("{idAdvert:guid}")]
    public async Task<IActionResult> SoftDeleteAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        await advertService.SoftDeleteAdvert(idAdvert, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    ///     Endpoint used for retrieving an idAdvert assigned to a user with a specified idUser.
    /// </summary>
    /// <param name="idUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpGet("id-advert/{idUser:guid}")]
    public async Task<IActionResult> GetAdvertIdByUserId(Guid idUser, CancellationToken cancellationToken)
    {
        Guid? advertIdAdvertByIdUser = await advertService.GetAdvertIdAdvertByIdUser(idUser, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, advertIdAdvertByIdUser);
    }

    /// <summary>
    ///     Endpoint used for retrieving advert associated data by the idUser.
    /// </summary>
    /// <param name="idUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("details/{idUser:guid}")]
    public async Task<IActionResult> GetAdvertAssociatedDataByIdUser(Guid idUser, CancellationToken cancellationToken)
    {
        GetAdvertDetailsResponse getAdvertResponse =
            await advertService.GetAdvertAssociatedDataByIdUser(idUser, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, getAdvertResponse);
    }

    /// <summary>
    ///     Endpoint used for retrieving advert associated data by the idAdvert.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{idAdvert:guid}")]
    public async Task<IActionResult> GetAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        GetAdvertDetailsResponse getAdvertResponse =
            await advertService.GetAdvertAssociatedDataByIdAdvert(idAdvert, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, getAdvertResponse);
    }

    /// <summary>
    ///     Endpoint used for getting all adverts with pagination and optional search functionality.
    /// </summary>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchTerm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAdvertsSummaries(string? sortOrder, int page, int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        PagedListDto<AdvertOverviewDto> getAllAdvertsResponse =
            await advertService.GetAllAdvertsSummaries(sortOrder, page, pageSize, searchTerm, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, getAllAdvertsResponse);
    }

    /// <summary>
    ///     Endpoint used for uploading mock images for testing purposes.
    /// </summary>
    /// <param name="coverImageFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("mock-image-upload")]
    public async Task<IActionResult> MockImageUpload(IFormFile coverImageFile,
        CancellationToken cancellationToken)
    {
        Guid mockImageUploadResponse = await advertService.MockImageUpload(coverImageFile, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, mockImageUploadResponse);
    }

    /// <summary>
    ///     Endpoint used for adding a review to a specified advert.
    /// </summary>
    /// <param name="addReviewRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Client")]
    [HttpPost("review")]
    public async Task<IActionResult> AddReview(AddReviewRequest addReviewRequest, CancellationToken cancellationToken)
    {
        Guid addReview = await advertService.AddReview(addReviewRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, addReview);
    }

    /// <summary>
    ///     Endpoint used for retrieving reviews for a specified advert.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("reviews")]
    public async Task<IActionResult> GetReviewsForAdvertPaginated(Guid idAdvert, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        PagedListDto<ReviewDto> reviewsForAdvertPaginated = await advertService.GetReviewsForAdvertPaginated(idAdvert, page, pageSize, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, reviewsForAdvertPaginated);
    }
}