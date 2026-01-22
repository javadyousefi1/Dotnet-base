using Application.Common.Models;
using MediatR;

namespace Application.Users.GetAllUsers;

public sealed record GetAllUsersQuery(
    int Page,
    int PageSize
) : IRequest<PaginatedResult<UserDto>>;
