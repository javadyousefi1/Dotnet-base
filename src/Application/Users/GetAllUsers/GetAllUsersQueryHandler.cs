using Application.Common.Abstractions;
using Application.Common.Models;
using MediatR;

namespace Application.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetAllAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        var userDtos = users.Select(u => new UserDto(
            u.Id,
            u.PhoneNumber,
            u.Email,
            u.Name,
            u.Family,
            u.Roles,
            u.CreatedAt
        )).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PaginatedResult<UserDto>(
            userDtos,
            request.Page,
            request.PageSize,
            totalCount,
            totalPages
        );
    }
}
