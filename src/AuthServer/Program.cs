using AuthServer.configs;
using Serilog;

/* Logger provis�rio
 * Escreve logs no console - �til em ambientes como desenvolvimento ou docker
*/
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

// Ponto de partida para incluir configua��es no projeto - Prepara a aplica��o para ser executada
var builder = WebApplication.CreateBuilder(args);

/* Configura Serilog como provedor de logging principal
 * Escreve logs no console de forma personalizada
*/
builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")  // Mensagem personalizada
        .Enrich.FromLogContext()  // Informa��es ou controler espec�fica de uma request
        .ReadFrom.Configuration(ctx.Configuration));

/* Injeta as configura��es do servidor de autentica��o
 * Configura��es determinadas em uma classe fora do program por quest�es de organiza��o
*/
var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// Inicia a aplica��o web
app.Run();
