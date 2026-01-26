using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Description
            }),
            ErrorType.NotFound => new NotFoundObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Description
            }),
            ErrorType.Conflict => new ConflictObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Description
            }),
            ErrorType.Unauthorized => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Description
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            },
            ErrorType.Forbidden => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Description
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            },
            _ => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Description
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}
