using Duende.IdentityServer.Services;
using Serilog;

namespace AuthServer.configs;

// Classe estática de configuação
internal static class HostingExtensions
{
    // Injeta as configurações do nosso identityserver
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Adiciona o identityserver na injeção de dependências do projeto
        var isBuilder = builder.Services.AddIdentityServer(options =>
        {
            /* Faz com que o identityserver dispare eventos
             * Erros, Sucessos e Falhas
            */
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.EmitStaticAudienceClaim = true;     // Garante que a claim "aud" seja incluída no token JWT
        })
            .AddTestUsers(TestUsers.Users);     // adiciona usuarios para testes

        /* Recursos de identidade em memória
         * São informações do usuário autenticado que podem ser incluídas no token 
         * O identity saberá que esses recursos poderão ser solicitado pelo client
        */
        isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
        /* Recursos de scopes em memória
         * São permissões que um client terá
         * Esses scopes serão tratados na sua API (Limitará se determinado scope pode acessar aquele endpoint)
        */
        isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
        /* Clients que podem se autenticar e obter tokens neste servidor
         * Pode ser um SPA (Angular, React, ETC), app mobile, backend ETC.
        */
        isBuilder.AddInMemoryClients(Config.Clients);

        /* Adiciona serviço de autenticação
         * Necessãrio para a aplicação reconhecer tokens, cookies etc.
        */
        builder.Services.AddAuthentication();
        // Adiciona serviço de autorização - Validar se o usuário tem permissão para acessar o recurso
        builder.Services.AddAuthorization();

        // Serviço personalizado para incluir claims nos token de acesso
        builder.Services.AddTransient<IProfileService, CustomProfileService>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Ativa o log das requisições
        app.UseSerilogRequestLogging();

        // Habilita o roteamento na aplicação
        app.UseRouting();

        // Ativa o Identity Server
        app.UseIdentityServer();

        return app;
    }
}
