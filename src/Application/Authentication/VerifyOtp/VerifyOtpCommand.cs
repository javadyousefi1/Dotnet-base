using MediatR;

namespace Application.Authentication.VerifyOtp;

public sealed record VerifyOtpCommand(
    string PhoneNumber,
    string Otp,
    string? Name = null,
    string? Family = null
) : IRequest<VerifyOtpResult>;
