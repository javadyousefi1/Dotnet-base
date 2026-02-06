using MediatR;
using SharedKernel;

namespace Application.Authentication.GetOtp;

public sealed record GetOtpCommand(
    string PhoneNumber
) : IRequest<Result<GetOtpResult>>;
