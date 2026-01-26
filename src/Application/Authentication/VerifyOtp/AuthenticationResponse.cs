namespace Application.Authentication.VerifyOtp;

public sealed record AuthenticationResponse(
    string Token,
    DateTime ExpiresAt
);
