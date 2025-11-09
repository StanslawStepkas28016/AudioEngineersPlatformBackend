using API.Abstractions;
using API.Contracts.Advert.Commands.AddReview;
using API.Contracts.Advert.Commands.ChangeAdvertData;
using API.Contracts.Advert.Commands.CreateAdvert;
using API.Contracts.Advert.Commands.DeleteAdvert;
using API.Contracts.Advert.Queries.GetAdvertDetails;
using API.Contracts.Advert.Queries.GetAdvertReviews;
using API.Contracts.Advert.Queries.GetAllAdvertsSummaries;
using API.Contracts.Advert.Queries.GetIdAdvertByIdUser;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.AddReview;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.CreateAdvert;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.DeleteAdvert;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertDetails;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;
using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetIdAdvertByIdUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/advert")]
public class AdvertController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly IClaimsUtil _claimsUtil;

    public AdvertController(
        IMapper mapper,
        ISender sender,
        IClaimsUtil claimsUtil
    )
    {
        _mapper = mapper;
        _sender = sender;
        _claimsUtil = claimsUtil;
    }

    [Authorize(Roles = "Administrator, Audio engineer")]
    [HttpPost]
    public async Task<IActionResult> CreateAdvert(
        [FromForm] CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != createAdvertRequest.IdUser && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot create this resource.");
        }

        // Map to command.
        CreateAdvertCommand command = _mapper.Map<CreateAdvertRequest, CreateAdvertCommand>(createAdvertRequest);

        // Send to mediator.
        CreateAdvertCommandResult result = await _sender.Send(command, cancellationToken);

        // Map result to response.
        CreateAdvertResponse response = _mapper.Map<CreateAdvertCommandResult, CreateAdvertResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Audio engineer")]
    [HttpPatch("{idAdvert:guid}")]
    public async Task<IActionResult> ChangeAdvertData(
        [FromRoute] Guid idAdvert,
        [FromBody] ChangeAdvertDataRequest changeAdvertDataRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != changeAdvertDataRequest.IdUser && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot edit this resource.");
        }

        // Map to command.
        ChangeAdvertDataCommand command = _mapper.Map<ChangeAdvertDataRequest, ChangeAdvertDataCommand>
        (
            changeAdvertDataRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.IdAdvert = idAdvert
            )
        );

        // Send to mediator.
        ChangeAdvertDataCommandResult result = await _sender.Send(command, cancellationToken);

        // Map result to response.
        ChangeAdvertDataResponse response = _mapper.Map<ChangeAdvertDataCommandResult, ChangeAdvertDataResponse>
            (result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Audio engineer")]
    [HttpDelete("{idAdvert:guid}")]
    public async Task<IActionResult> DeleteAdvert(
        [FromRoute] Guid idAdvert,
        [FromBody] DeleteAdvertRequest deleteAdvertRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUser = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUser != deleteAdvertRequest.IdUser && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot delete this resource.");
        }

        // Map request to command.
        DeleteAdvertCommand command = _mapper.Map<DeleteAdvertRequest, DeleteAdvertCommand>
        (
            deleteAdvertRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.IdAdvert = idAdvert
            )
        );

        // Send to mediator.
        DeleteAdvertCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        DeleteAdvertResponse response = _mapper.Map<DeleteAdvertCommandResult, DeleteAdvertResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Audio engineer")]
    [HttpGet("{idUser:guid}/id-advert")]
    public async Task<IActionResult> GetIdAdvertByIdUser(
        [FromRoute] Guid idUser,
        CancellationToken cancellationToken
    )
    {
        // Map route/path param to request.
        GetIdAdvertByIdUserRequest request = _mapper.Map<Guid, GetIdAdvertByIdUserRequest>(idUser);

        // Map to query.
        GetIdAdvertByIdUserQuery query = _mapper.Map<GetIdAdvertByIdUserRequest, GetIdAdvertByIdUserQuery>
            (request);

        // Send to mediator.
        GetIdAdvertByIdUserQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetIdAdvertByIdUserResponse response = _mapper.Map<GetIdAdvertByIdUserQueryResult, GetIdAdvertByIdUserResponse>
            (result);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("summaries")]
    public async Task<IActionResult> GetAllAdvertsSummaries(
        [FromQuery] GetAllAdvertsSummariesRequest getAllAdvertsSummariesRequest,
        CancellationToken cancellationToken
    )
    {
        // Map to query.
        GetAllAdvertsSummariesQuery query = _mapper.Map<GetAllAdvertsSummariesRequest, GetAllAdvertsSummariesQuery>
            (getAllAdvertsSummariesRequest);

        // Send to mediator.
        GetAllAdvertsSummariesQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetAllAdvertsSummariesResponse response =
            _mapper.Map<GetAllAdvertsSummariesQueryResult, GetAllAdvertsSummariesResponse>(result);

        return Ok(response.PagedAdvertSummaries);
    }

    [AllowAnonymous]
    [HttpGet("{idAdvert:guid}/details")]
    public async Task<IActionResult> GetAdvertDetails(
        [FromRoute] Guid idAdvert,
        CancellationToken cancellationToken
    )
    {
        // Map to request.
        GetAdvertDetailsRequest request = _mapper.Map<Guid, GetAdvertDetailsRequest>(idAdvert);

        // Map to query.
        GetAdvertDetailsQuery query = _mapper.Map<GetAdvertDetailsRequest, GetAdvertDetailsQuery>
            (request);

        // Send to mediator.
        GetAdvertDetailsQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetAdvertDetailsResponse response = _mapper.Map<GetAdvertDetailsQueryResult, GetAdvertDetailsResponse>(result);

        return Ok(response);
    }

    [Authorize(Roles = "Administrator, Client")]
    [HttpPost("{idAdvert:guid}/review")]
    public async Task<IActionResult> AddReview(
        [FromRoute] Guid idAdvert,
        [FromBody] AddReviewRequest addReviewRequest,
        CancellationToken cancellationToken
    )
    {
        // Extract the idUser with roleName and perform authorization checks.
        Guid idUserFromClaims = await _claimsUtil.ExtractIdUserFromClaims();
        string roleNameFromClaims = await _claimsUtil.ExtractRoleNameFromClaims();

        if (idUserFromClaims != addReviewRequest.IdUserReviewer && roleNameFromClaims != "Administrator")
        {
            throw new UnauthorizedAccessException("You cannot create this resource.");
        }

        // Map to command.
        AddReviewCommand command = _mapper.Map<AddReviewRequest, AddReviewCommand>
        (
            addReviewRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.IdAdvert = idAdvert
            )
        );

        // Send to mediator.
        AddReviewCommandResult result = await _sender.Send(command, cancellationToken);

        // Map to response.
        AddReviewResponse response = _mapper.Map<AddReviewCommandResult, AddReviewResponse>(result);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("{idAdvert:guid}/reviews")]
    public async Task<IActionResult> GetAdvertReviews(
        [FromRoute] Guid idAdvert,
        [FromQuery] GetAdvertReviewsRequest getAdvertReviewsRequest,
        CancellationToken cancellationToken
    )
    {
        // Map to query.
        GetAdvertReviewsQuery query = _mapper.Map<GetAdvertReviewsRequest, GetAdvertReviewsQuery>
        (
            getAdvertReviewsRequest,
            opt => opt.AfterMap
            ((
                    _,
                    dest
                ) => dest.IdAdvert = idAdvert
            )
        );

        // Send to mediator.
        GetAdvertReviewsQueryResult result = await _sender.Send(query, cancellationToken);

        // Map to response.
        GetAdvertReviewsResponse response = _mapper.Map<GetAdvertReviewsQueryResult, GetAdvertReviewsResponse>(result);

        return Ok(response.PagedAdvertReviews);
    }
}