using Application.Authentication.GetOtp;
using Domain.Enums;
using FluentValidation;

namespace Application.Users.Update;

public class RoleMangementCommandValidator : AbstractValidator<RoleMangementCommand>
{
    public RoleMangementCommandValidator()
    {
        RuleFor(x => x.userId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.roles)
            .NotNull()
            .WithMessage("Roles list cannot be null.");

        RuleForEach(x => x.roles)
            .IsInEnum()
            .WithMessage("Invalid role value. Must be a valid UserRole.");
    }
}