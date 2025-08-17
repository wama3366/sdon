using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SchoolDonations.UserInterface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController :ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var name = User.FindFirst("name")?.Value ?? User.FindFirst("sub")?.Value;
        return new JsonResult(new { message = "Local API Success!", user = name });
    }
}