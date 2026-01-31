using Application.Common.Abstractions;
using MediatR;
using SharedKernel;

namespace Application.Users.Update;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    private readonly IUserRepository _userRepository;
    
    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = _userRepository;
    }

    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _userRepository.GetByIdAsync()
        await _userRepository.UpdateInfoAsync()
    }
    
}
