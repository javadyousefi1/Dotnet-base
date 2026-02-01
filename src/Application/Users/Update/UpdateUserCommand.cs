using MediatR;
using SharedKernel;

namespace Application.Users.Update;

public sealed record UpdateUserCommand(
    string FirstName,
    string LastName,
    string? Email = null
) : IRequest<Result<UpdateUserResponse>>;
