using Microsoft.IdentityModel.Tokens;
using System.Text;
using Asp.Versioning;

namespace UserManagement.API.configs;

public static class Configurations
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication()
            .AddJwtBearer(opt =>
            {
                opt.Authority = config.GetSection("AuthServer:authority").Value; // URL servidor de autenticação (usado para validar issuer)
                opt.TokenValidationParameters = new TokenValidationParameters  // configuração de regras de validação do JWT
                {
                    ValidAudience = config.GetSection("AuthServer:audience").Value,  // valida o aud que o token deve conter
                    ValidateAudience = true,  // valida se o aud no token é igual ao ValidAudience
                    ValidateIssuer = true,  // valida se o issuer no token é igual ao ValidIssuer
                    ValidIssuer = config.GetSection("AuthServer:audience").Value,
                    ValidateIssuerSigningKey = true,  // habilitado para validar assinatura do token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AuthServer:secret").Value!))  // valida se o token foi assinado com a chave fornecida
                };
            });

        services.AddAuthorization();
    }

    public static void AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
