using Application.Authentication.GetOtp;
using Application.Authentication.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("getOtp")]
    public async Task<IActionResult> GetOtp([FromBody] GetOtpRequest request, CancellationToken cancellationToken)
    {
        var command = new GetOtpCommand(request.PhoneNumber);
        var result = await _sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost("verifyOtp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request, CancellationToken cancellationToken)
    {
        var command = new VerifyOtpCommand(
            request.PhoneNumber,
            request.Otp,
            request.Name,
            request.Family);

        var result = await _sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }
}

public sealed record GetOtpRequest(string PhoneNumber);

public sealed record VerifyOtpRequest(
    string PhoneNumber,
    string Otp,
    string? Name = null,
    string? Family = null);
