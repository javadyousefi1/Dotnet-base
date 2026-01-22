using Application.Common.Abstractions;
using MediatR;

namespace Application.Authentication.GetOtp;

public sealed class GetOtpCommandHandler : IRequestHandler<GetOtpCommand, GetOtpResult>
{
    private readonly IOtpCache _otpCache;

    public GetOtpCommandHandler(IOtpCache otpCache)
    {
        _otpCache = otpCache;
    }

    public async Task<GetOtpResult> Handle(GetOtpCommand request, CancellationToken cancellationToken)
    {
        var otp = GenerateOtp();
        var expiration = TimeSpan.FromMinutes(2);

        await _otpCache.SetOtpAsync(request.PhoneNumber, otp, expiration, cancellationToken);

        return new GetOtpResult(true, $"OTP sent successfully. (Development: {otp})");
    }

    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
