using Duende.IdentityModel;
using Duende.IdentityServer.Models;

namespace AuthServer.configs;

internal static class Config
{
    // Define informações do usuário (claims) que estão disponíveis para os clients
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    // Define os scopes de API (permissões) que cada client pode ter
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("apis_study_project")
            {
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role
                }
            },
            new ApiScope("app")
            {
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Role
                }
            },
            new ApiScope("pwa")
            {
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Role
                }
            }
        };

    /* Define os clients que poderão se autenticar nesse provedor
     ***************  ATENÇÃO  ***************
     * Não se limite a essas configurações, você pode ter vários clients diferentes (por exemplo, 3 apps, 2 SPA, 4 API's ETC...)
     * As configurações abaixo são exemplos de clients
    */
    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            /* Exemplo de um client frontend
             * Frontend onde o usuário estará vizualizando nossos serviços
            */
            new Client
            {
                ClientId = "my.app.cliente",  // Id do client (deve ser único)
                ClientName = "Frontend Cliente",  // Nome do client para ser identificado de forma fácil"
                AllowedGrantTypes = GrantTypes.Code,  // Tipo de fluxo de autenticação OAuth 2.0 (usado em aplicações SPA, o usuário realizando o login obtem um cõdigo que será trocado por um token JWT)
                RequirePkce = true,  // Habilita camada extra de segurança no fluxo de autenticação code
                RequireClientSecret = false,    // Desabilita secret pois, quando o client é um SPA (angular, react etc) e a secret fica exporta no navegador, sendo assim a secret fica "exposta"
                RedirectUris = { "https://localhost:8080/callback" },    // URL que o usuário será redirecionado após login com sucesso
                PostLogoutRedirectUris = { "https://localhost:8080/" }, // URL que o usuário será redirecionado apõs logout
                AllowedCorsOrigins = { "https://localhost:8080" },  // lista de domínios que o IdentityServer4 vai permitir requisições CORS
                AllowedScopes = { "apis_exemplo", "profile", "openid" },  // Escopos que esse cliente tem permissão de acesso nas nossas API's (deve ser configurado as permissões na API)
                AllowAccessTokensViaBrowser = true,  // Permite que os tokens de acesso sejam entregues ao navegador (client)
                AllowOfflineAccess = true,  // Permite solicitação de Refresh token
                AlwaysIncludeUserClaimsInIdToken = true  // Faz com que todas as claims do usuário sejam incluídas no id_token
            },

            /* Exemplo de um client backend
             * Uma api externa que poderá consumir seus serviços para oferecer-los através da plataforma dele
            */
            new Client
            {
                ClientId = config.GetSection("Clients:Apis_Example:client-id").Value!,  // Id do client (deve ser único)
                ClientName = "API's de estudos",  // Nome do client para ser identificado de forma fácil
                AllowedGrantTypes = GrantTypes.ClientCredentials,  // Tipo de fluxo de autenticação OAuth 2.0 (usado quando não há um usuário humano se autenticando)
                ClientSecrets = { new Secret(config.GetSection("Clients:Apis_Example:secret").Value.Sha256()) },  // Segredo que o usuário usará para se logar (digamos que é uma senha)
                
                /* Dealhes importantes do AllowedScopes
                 * O que eu definir abaixo (por exemplo "email", "profile", "openid") não será incluso no access_token, mas sim no id_token
                 * O scope deve estar definido no IdentityResource caso contrário será gerado um erro
                */ 
                AllowedScopes = { "apis_study_project", "profile", "openid" },  // Escopos que esse cliente tem permissão de acesso nas nossas API's (deve ser configurado as permissões na API)
            },

            // Exemplo de com um endpoint sendo o "provedor" da identificação (recebendo as credenciais do cliente e repassando para o auth server)
            new Client
            {
                ClientId = config.GetSection("Clients:app:client-id").Value!,  // Id do client (deve ser único)
                ClientName = "Autenticação via APP",  // Nome do client para ser identificado de forma fácil
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,  // Tipo de fluxo de autenticação OAuth 2.0 (usado quando não há um usuário humano se autenticando)
                ClientSecrets = { new Secret(config.GetSection("Clients:app:secret").Value.Sha256()) },  // Segredo que o usuário usará para se logar (digamos que é uma senha)

                /* Dealhes importantes do AllowedScopes
                 * O que eu definir abaixo (por exemplo "email", "profile", "openid") não será incluso no access_token, mas sim no id_token
                 * O scope deve estar definido no IdentityResource caso contrário será gerado um erro
                */ 
                AllowedScopes = { "app", "profile", "openid" },  // Escopos que esse cliente tem permissão de acesso nas nossas API's (deve ser configurado as permissões na API)
            },

            // Exemplo de com um endpoint sendo o "provedor" da identificação (recebendo as credenciais do cliente e repassando para o auth server)
            new Client
            {
                ClientId = config.GetSection("Clients:pwa:client-id").Value!,  // Id do client (deve ser único)
                ClientName = "Autenticação via PWA",  // Nome do client para ser identificado de forma fácil
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,  // Tipo de fluxo de autenticação OAuth 2.0 (usado quando não há um usuário humano se autenticando)
                ClientSecrets = { new Secret(config.GetSection("Clients:pwa:secret").Value.Sha256()) },  // Segredo que o usuário usará para se logar (digamos que é uma senha)

                /* Dealhes importantes do AllowedScopes
                 * O que eu definir abaixo (por exemplo "email", "profile", "openid") não será incluso no access_token, mas sim no id_token
                 * O scope deve estar definido no IdentityResource caso contrário será gerado um erro
                */ 
                AllowedScopes = { "pwa", "profile", "openid" },  // Escopos que esse cliente tem permissão de acesso nas nossas API's (deve ser configurado as permissões na API)
            },
        };
}
