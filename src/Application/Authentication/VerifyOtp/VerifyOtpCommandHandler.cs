using Application.Common.Abstractions;
using Domain.Entities;
using Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Authentication.VerifyOtp;

public sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result<AuthenticationResponse>>
{
    private readonly IOtpCache _otpCache;
    private readonly IAppDbContext _dbContext;
    private readonly IJwtProvider _jwtProvider;

    public VerifyOtpCommandHandler(
        IOtpCache otpCache,
        IAppDbContext dbContext,
        IJwtProvider jwtProvider)
    {
        _otpCache = otpCache;
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthenticationResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var storedOtp = await _otpCache.GetOtpAsync(request.PhoneNumber, cancellationToken);

        if (storedOtp is null || storedOtp != request.Otp)
        {
            return Result.Failure<AuthenticationResponse>(UserErrors.InvalidOtp());
        }

        await _otpCache.RemoveOtpAsync(request.PhoneNumber, cancellationToken);

        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (user is null)
        {
            user = User.Create(
                request.PhoneNumber,
                request.Name ?? string.Empty,
                request.Family ?? string.Empty);

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var token = _jwtProvider.Generate(user.Id, user.Roles);

        var response = new AuthenticationResponse(token.Token, token.ExpiresAt);

        return Result.Success(response);
    }
}
