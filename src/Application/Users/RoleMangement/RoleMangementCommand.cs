using Application.Users.Update;
using Domain.Enums;
using MediatR;
using SharedKernel;

namespace Application.Authentication.GetOtp;

public sealed record RoleMangementCommand(
    Guid userId,
    List<UserRole> roles
) : IRequest<Result<RoleMangementResponse>>;
