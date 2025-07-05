using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Advert;
using AudioEngineersPlatformBackend.Contracts.Advert.Create;
using AudioEngineersPlatformBackend.Contracts.Advert.Edit;
using AudioEngineersPlatformBackend.Domain.Entities;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/advert")]
public class AdvertController(IAdvertService advertService) : ControllerBase
{
    /// <summary>
    ///     Endpoint for creating an advert.
    /// </summary>
    /// <param name="createAdvertRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPost]
    public async Task<IActionResult> CreateAdvert([FromForm] CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        var createAdvertResponse = await advertService.CreateAdvert(createAdvertRequest, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, createAdvertResponse);
    }

    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpPatch("{idAdvert:guid}")]
    public async Task<IActionResult> EditAdvert(Guid idAdvert, EditAdvertRequest editAdvertRequest,
        CancellationToken cancellationToken)
    {
        await advertService.EditAdvert(idAdvert, editAdvertRequest, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    ///     Method for soft-deleting an advert.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin, Audio engineer")]
    [HttpDelete("{idAdvert:guid}")]
    public async Task<IActionResult> DeleteAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        await advertService.DeleteAdvert(idAdvert, cancellationToken);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUserAdvert(Guid idUser, CancellationToken cancellationToken)
    {
        var getAdvertResponse = await advertService.GetUserAdvert(idUser, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, getAdvertResponse);
    }

    // TODO: Implement the method to get advert details by idAdvert.
    // public async Task<IActionResult> GetAdvertById(Guid idAdvert, CancellationToken cancellationToken)
    // {
    //     throw new NotImplementedException("This method is not implemented yet.");
    // }

    /// <summary>
    ///     Endpoint for getting all adverts with pagination and optional search functionality.
    /// </summary>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchTerm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAdverts(string? sortOrder, int page, int pageSize, string? searchTerm,
        CancellationToken cancellationToken)
    {
        var getAllAdvertsResponse =
            await advertService.GetAllAdverts(sortOrder, page, pageSize, searchTerm, cancellationToken);
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
        var mockImageUploadResponse = await advertService.MockImageUpload(coverImageFile, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, mockImageUploadResponse);
    }
}