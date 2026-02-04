using Application.Common.Abstractions;
using Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Update;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserCommandHandler(
        IAppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == Guid.Empty)
        {
            return Result.Failure<UpdateUserResponse>(Error.Unauthorized(
                "User.Unauthorized",
                "User is not authenticated"));
        }

        if (request.FirstName is null && request.LastName is null && request.Email is null)
        {
            return Result.Failure<UpdateUserResponse>(Error.Validation(
                "User.NoFieldsToUpdate",
                "At least one field must be provided for update"));
        }

        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.NotFound(userId));
        }

        if (request.FirstName is not null && string.IsNullOrWhiteSpace(request.FirstName))
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.InvalidName());
        }

        if (request.LastName is not null && string.IsNullOrWhiteSpace(request.LastName))
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.InvalidFamily());
        }

        user.UpdateProfilePartial(request.FirstName, request.LastName, request.Email);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserResponse(user.Id.ToString()));
    }
}
