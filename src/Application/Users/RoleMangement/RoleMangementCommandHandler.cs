using Application.Authentication.GetOtp;
using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Update;

public sealed class RoleMangementCommandHandler : IRequestHandler<RoleMangementCommand, Result<RoleMangementResponse>>
{
    private readonly IAppDbContext _dbContext;

    public RoleMangementCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<RoleMangementResponse>> Handle(RoleMangementCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _dbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == request.userId);

        if (userInfo is null)
        {
            return Result.Failure<RoleMangementResponse>(Error.NotFound("User.NotFound" , "User Not Found"));
        }

        userInfo.AddRoles(request.roles);
        _dbContext.Users.Update(userInfo);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(new RoleMangementResponse(request.roles));
    }
}
