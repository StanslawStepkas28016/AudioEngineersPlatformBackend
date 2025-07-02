using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Advert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/advert")]
public class AdvertController : ControllerBase
{
    private readonly IAdvertService _advertService;

    public AdvertController(IAdvertService advertService)
    {
        _advertService = advertService;
    }

    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAdvert([FromForm] CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        var createAdvertResponse = await _advertService.CreateAdvert(createAdvertRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, createAdvertResponse);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUserAdvert(Guid idUser, CancellationToken cancellationToken)
    {
        var getAdvertResponse = await _advertService.GetUserAdvert(idUser, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, getAdvertResponse);
    }

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAdverts(string? sortOrder, int page, int pageSize, string? searchTerm,
        CancellationToken cancellationToken)
    {
        var getAllAdvertsResponse =
            await _advertService.GetAllAdverts(sortOrder, page, pageSize, searchTerm, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, getAllAdvertsResponse);
    }

    /// <summary>
    ///     Method used for uploading mock images for testing purposes.
    /// </summary>
    /// <param name="coverImageFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("mock-image-upload")]
    public async Task<IActionResult> MockImageUpload(IFormFile coverImageFile,
        CancellationToken cancellationToken)
    {
        var mockImageUploadResponse = await _advertService.MockImageUpload(coverImageFile, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, mockImageUploadResponse);
    }

    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPost("delete/{idAdvert:guid}")]
    public async Task<IActionResult> DeleteAdvert(Guid idAdvert, CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This method is not implemented yet.");
    }

    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPatch("edit/{idAdvert:guid}")]
    public async Task<IActionResult> EditAdvert(Guid idAdvert, CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This method is not implemented yet.");
    }
}