namespace Application.Common.Abstractions;

public interface IOtpCache
{
    Task SetOtpAsync(string identifier, string otp, TimeSpan expiration, CancellationToken cancellationToken = default);
    Task<string?> GetOtpAsync(string identifier, CancellationToken cancellationToken = default);
    Task RemoveOtpAsync(string identifier, CancellationToken cancellationToken = default);
}
