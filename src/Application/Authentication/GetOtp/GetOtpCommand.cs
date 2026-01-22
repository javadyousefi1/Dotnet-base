using MediatR;

namespace Application.Authentication.GetOtp;

public sealed record GetOtpCommand(
    string PhoneNumber
) : IRequest<GetOtpResult>;
