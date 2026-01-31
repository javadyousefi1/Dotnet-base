using Application.Authentication.VerifyOtp;
using MediatR;
using SharedKernel;

namespace Application.Users.Update;

public sealed class UpdateUserCommand(
    string FirstName,
    string LastName
) : IRequest<Result<UpdateUserResponse>>;
