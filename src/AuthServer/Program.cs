using AuthServer.configs;
using Serilog;

/* Logger provisõrio
 * Escreve logs no console - Útil em ambientes como desenvolvimento ou docker
*/
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

// Ponto de partida para incluir configuações no projeto - Prepara a aplicação para ser executada
var builder = WebApplication.CreateBuilder(args);

/* Configura Serilog como provedor de logging principal
 * Escreve logs no console de forma personalizada
*/
builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")  // Mensagem personalizada
        .Enrich.FromLogContext()  // Informações ou controler específica de uma request
        .ReadFrom.Configuration(ctx.Configuration));

/* Injeta as configurações do servidor de autenticação
 * Configurações determinadas em uma classe fora do program por questões de organização
*/
var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// Inicia a aplicação web
app.Run();
