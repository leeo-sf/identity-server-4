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
            new ApiScope("apis_study_project"),
            new ApiScope("apis_exemplo")
        };

    /* Define os clients que poderão se autenticar nesse provedor
     ***************  ATENÇÃO  ***************
     * Não se limite a essas configurações, você pode ter vários clients diferentes (por exemplo, 3 apps, 2 SPA, 4 API's ETC...)
     * As configurações abaixo são exemplos de clients
    */
    public static IEnumerable<Client> Clients =>
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
                ClientId = "apis.study.project",  // Id do client (deve ser único)
                ClientName = "API's de estudos",  // Nome do client para ser identificado de forma fácil
                AllowedGrantTypes = GrantTypes.ClientCredentials,  // Tipo de fluxo de autenticação OAuth 2.0 (usado quando não há um usuário humano se autenticando)
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },  // Segredo que o usuário usará para se logar (digamos que é uma senha)
                AllowedScopes = { "apis_study_project" }  // Escopos que esse cliente tem permissão de acesso nas nossas API's (deve ser configurado as permissões na API)
            }
        };
}
