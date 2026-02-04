using Application.Common.Abstractions;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllUsersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users
            .Include(u => u.UserRoles)
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

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
