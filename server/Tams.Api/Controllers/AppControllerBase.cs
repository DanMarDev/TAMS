using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tams.Api.Controllers;

public abstract class AppControllerBase : ControllerBase
{
    protected int GetUserIdFromClaims()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}