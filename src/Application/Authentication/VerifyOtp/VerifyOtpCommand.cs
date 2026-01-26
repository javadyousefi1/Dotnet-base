using MediatR;
using SharedKernel;

namespace Application.Authentication.VerifyOtp;

public sealed record VerifyOtpCommand(
    string PhoneNumber,
    string Otp,
    string? Name = null,
    string? Family = null
) : IRequest<Result<AuthenticationResponse>>;
