using Application.Common.Abstractions;
using Domain.Errors;
using MediatR;
using SharedKernel;

namespace Application.Users.Update;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
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

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.NotFound(userId));
        }

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.InvalidName());
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.InvalidFamily());
        }

        user.UpdateProfile(request.FirstName, request.LastName, request.Email);

        await _userRepository.UpdateInfoAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserResponse(user.Id.ToString()));
    }
}
