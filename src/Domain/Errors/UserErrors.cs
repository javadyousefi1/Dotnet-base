using SharedKernel;

namespace Domain.Errors;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "User.NotFound",
        $"User with ID '{userId}' was not found");

    public static Error NotFoundByPhoneNumber(string phoneNumber) => Error.NotFound(
        "User.NotFoundByPhoneNumber",
        $"User with phone number '{phoneNumber}' was not found");

    public static Error InvalidPhoneNumber(string phoneNumber) => Error.Validation(
        "User.InvalidPhoneNumber",
        $"Phone number '{phoneNumber}' is invalid");

    public static Error InvalidOtp() => Error.Validation(
        "User.InvalidOtp",
        "Invalid or expired OTP");

    public static Error AlreadyExists(string phoneNumber) => Error.Conflict(
        "User.AlreadyExists",
        $"User with phone number '{phoneNumber}' already exists");

    public static Error InvalidName() => Error.Validation(
        "User.InvalidName",
        "User name cannot be empty");

    public static Error InvalidFamily() => Error.Validation(
        "User.InvalidFamily",
        "User family name cannot be empty");
}
