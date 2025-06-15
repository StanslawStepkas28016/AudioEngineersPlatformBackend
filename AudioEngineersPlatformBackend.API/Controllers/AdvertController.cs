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
    public async Task<IActionResult> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        var createAdvertResponse = await _advertService.CreateAdvert(createAdvertRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, createAdvertResponse);
    }

    [AllowAnonymous]
    [HttpGet("get/{idAdvert:guid}")]
    public async Task<IActionResult> GetAdvert(Guid idAdvert, CancellationToken cancellationToken)
    {
        // var getAdvertResponse = await _advertService.GetAdvert(idAdvert, cancellationToken);
        // return StatusCode(StatusCodes.Status200OK, getAdvertResponse);
        throw new NotImplementedException("This method is not implemented yet.");
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
        // var editAdvertResponse = await _advertService.EditAdvert(idAdvert, createAdvertRequest, cancellationToken);
        // return StatusCode(StatusCodes.Status200OK, editAdvertResponse);
    }

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAdverts(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This method is not implemented yet.");
        // var getAllAdvertsResponse = await _advertService.GetAllAdverts(cancellationToken);
        // return StatusCode(StatusCodes.Status200OK, getAllAdvertsResponse);
    }
}