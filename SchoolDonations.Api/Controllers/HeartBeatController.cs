using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolDonations.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HeartBeatController : ControllerBase
{
    #region Queries

    [HttpGet("identity")]
    [Authorize("SchoolDonations.Api.Read")]
    [Authorize("SchoolDonations.Api.Write")]
    public IActionResult GetUserClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }

    [HttpGet("check-access")]
    [Authorize("SchoolDonations.Api.Read")]
    [Authorize("SchoolDonations.Api.Write")]
    public IActionResult GetSuccess()
    {
        return Ok("Success");
    }

    #endregion Queries
}