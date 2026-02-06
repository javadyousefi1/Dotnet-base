using FluentValidation;
namespace Application.Authentication.GetOtp;

public sealed class GetOtpCommandValidator : AbstractValidator<GetOtpCommand>
{
    public GetOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^(\+98|0)?9\d{9}$")
            .WithMessage("Phone number must be a valid Iranian phone number");
    }
}
