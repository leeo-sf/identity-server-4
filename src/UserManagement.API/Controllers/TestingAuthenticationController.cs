using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[ControllerName("test-authentication")]
public class TestingAuthenticationController : ControllerBase
{
    [HttpGet("exemplo")]
    [Authorize(Policy = "apis.exemplo")]
    public async Task<IActionResult> ApisExemplo()
    {
        return Ok("Access allowed");
    }

    [HttpGet("study")]
    [Authorize(Policy = "apis.study")]
    public async Task<IActionResult> ApisStudy()
    {
        return Ok("Access allowed");
    }
}
