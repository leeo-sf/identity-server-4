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
    [HttpGet("pwa")]
    [Authorize(Policy = "apis.pwa")]
    public async Task<IActionResult> ApisExemplo()
    {
        return Ok("Access allowed");
    }

    [HttpGet("app")]
    [Authorize(Policy = "apis.app")]
    public async Task<IActionResult> ApisStudy()
    {
        return Ok("Access allowed");
    }

    [HttpGet("any-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AnyAdmin()
    {
        return Ok("Any admin user can access.");
    }

    [HttpGet("admin-with-scope-app")]
    [Authorize(Policy = "admin.scope.app")]
    public async Task<IActionResult> AdminWithScope()
    {
        return Ok("Only admin users with scope apis.app");
    }
}
