using Application.Common.Abstractions;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Authentication.VerifyOtp;

public sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResult>
{
    private readonly IOtpCache _otpCache;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public VerifyOtpCommandHandler(
        IOtpCache otpCache,
        IUserRepository userRepository,
        IJwtProvider jwtProvider)
    {
        _otpCache = otpCache;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<VerifyOtpResult> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var storedOtp = await _otpCache.GetOtpAsync(request.PhoneNumber, cancellationToken);

        if (storedOtp is null || storedOtp != request.Otp)
        {
            return new VerifyOtpResult(false, null, null, "Invalid or expired OTP");
        }

        await _otpCache.RemoveOtpAsync(request.PhoneNumber, cancellationToken);

        var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                PhoneNumber = request.PhoneNumber,
                Name = request.Name ?? string.Empty,
                Family = request.Family ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                Roles = new List<UserRole> { UserRole.User }
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        var token = _jwtProvider.Generate(user.Id, user.Roles);

        return new VerifyOtpResult(true, token.Token, token.ExpiresAt, "Authentication successful");
    }
}
