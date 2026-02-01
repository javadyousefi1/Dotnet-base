using MediatR;
using SharedKernel;

namespace Application.Users.Update;

public sealed record UpdateUserCommand(
    string? FirstName = null,
    string? LastName = null,
    string? Email = null
) : IRequest<Result<UpdateUserResponse>>;
