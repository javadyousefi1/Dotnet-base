using Application.Common.Abstractions;
using Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;


public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserCommandHandler(
        IAppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == Guid.Empty)
        {
            return Result.Failure(Error.Unauthorized(
                "User.Unauthorized",
                "User is not authenticated"));
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(userId));
        }

        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new { userId = user.Id });
    }
}
