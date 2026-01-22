namespace Application.Authentication.GetOtp;

public sealed record GetOtpResult(
    bool Success,
    string Message
);
