using Asp.Versioning;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ControllerName("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginAsCostumer()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_config.GetSection("AuthServer:authority").Value);
            if (disco.IsError)
            {
                return BadRequest(new { disco.Error });
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _config.GetSection("AuthServer:client-id").Value!,
                ClientSecret = _config.GetSection("AuthServer:secret").Value!,
                Scope = _config.GetSection("AuthServer:audience").Value!,
            });
            if (tokenResponse.IsError)
            {
                return BadRequest(new { disco.Error });
            }

            return Ok(new { tokenResponse.Json });
        }

        [HttpPost("app")]
        public async Task<IActionResult> LoginApp([FromBody] LoginRequest model)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_config.GetSection("app:authority").Value);
            if (disco.IsError)
            {
                return BadRequest(new { disco.Error });
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _config.GetSection("app:client-id").Value!,
                ClientSecret = _config.GetSection("app:secret").Value!,
                Scope = _config.GetSection("app:audience").Value!,
                UserName = model.Email,
                Password = model.Password
            });
            if (tokenResponse.IsError)
            {
                return BadRequest(new { disco.Error });
            }

            return Ok(new { tokenResponse.Json });
        }

        [HttpPost("pwa")]
        public async Task<IActionResult> LoginPwa([FromBody] LoginRequest model)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_config.GetSection("pwa:authority").Value);
            if (disco.IsError)
            {
                return BadRequest(new { disco.Error });
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _config.GetSection("pwa:client-id").Value!,
                ClientSecret = _config.GetSection("pwa:secret").Value!,
                Scope = _config.GetSection("pwa:audience").Value!,
                UserName = model.Email,
                Password = model.Password
            });
            if (tokenResponse.IsError)
            {
                return BadRequest(new { disco.Error });
            }

            return Ok(new { tokenResponse.Json });
        }
    }
}
