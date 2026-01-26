using FluentValidation;

namespace Application.Authentication.VerifyOtp;

public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^(\+98|0)?9\d{9}$")
            .WithMessage("Phone number must be a valid Iranian phone number");

        RuleFor(x => x.Otp)
            .NotEmpty()
            .WithMessage("OTP is required")
            .Length(6)
            .WithMessage("OTP must be 6 digits")
            .Matches(@"^\d{6}$")
            .WithMessage("OTP must contain only digits");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Family)
            .MaximumLength(100)
            .WithMessage("Family name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Family));
    }
}
