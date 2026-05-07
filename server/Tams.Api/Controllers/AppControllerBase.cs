using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tams.Api.Controllers;

public abstract class AppControllerBase : ControllerBase
{
    protected int GetUserIdFromClaims()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    protected IActionResult HandleException(Exception ex) => ex switch
    {
        ArgumentException => BadRequest(ex.Message),
        KeyNotFoundException => NotFound(ex.Message),
        UnauthorizedAccessException => Unauthorized(ex.Message),
        InvalidOperationException => BadRequest(ex.Message),
        _ => StatusCode(500, "An unexpected error occurred.")
    };
}