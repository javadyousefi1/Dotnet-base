namespace Application.Authentication.VerifyOtp;

public sealed record VerifyOtpResult(
    bool Success,
    string? Token,
    DateTime? ExpiresAt,
    string? Message
);
