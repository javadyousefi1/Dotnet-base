using Application.Common.Abstractions;
using MediatR;
using SharedKernel;

namespace Application.Authentication.GetOtp;

public sealed class GetOtpCommandHandler : IRequestHandler<GetOtpCommand, Result<GetOtpResult>>
{
    private readonly IOtpCache _otpCache;

    public GetOtpCommandHandler(IOtpCache otpCache)
    {
        _otpCache = otpCache;
    }

    public async Task<Result<GetOtpResult>> Handle(GetOtpCommand request, CancellationToken cancellationToken)
    {
        var otp = GenerateOtp();
        var expiration = TimeSpan.FromMinutes(2);

        await _otpCache.SetOtpAsync(request.PhoneNumber, otp, expiration, cancellationToken);

        return Result.Success(new GetOtpResult(true, otp));
    }

    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
