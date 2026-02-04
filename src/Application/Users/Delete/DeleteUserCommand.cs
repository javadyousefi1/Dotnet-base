using MediatR;
using SharedKernel;

public sealed record DeleteUserCommand(
    Guid UserId
) : IRequest<Result>;
