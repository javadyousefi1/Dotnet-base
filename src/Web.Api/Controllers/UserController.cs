using Application.Authentication.GetOtp;
using Application.Users.GetAllUsers;
using Application.Users.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public sealed class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var query = new GetAllUsersQuery(page, pageSize);
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserCommand(
            request.FirstName,
            request.LastName,
            request.Email
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser(
        [FromBody] DeleteUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand(
            request.UserId
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result);
    }
    
    [HttpPatch("RoleMangement")]
    public async Task<IActionResult> RoleMangementUser(
        [FromBody] RoleMangementCommand request,
        CancellationToken cancellationToken = default)
    {
        var command = new RoleMangementCommand(
            request.userId,
            request.roles
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result);
    }
}

public sealed record UpdateUserRequest(
    string? FirstName = null,
    string? LastName = null,
    string? Email = null
);

public sealed record DeleteUserRequest(
    Guid UserId
);
