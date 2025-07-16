using System.Text;
using BotDiscordLaoTon.Net.Helpers;
using BotDiscordLaoTon.Net.Options;
using BotDiscordLaoTon.Net.Services;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Serilog;

Console.OutputEncoding = Encoding.UTF8;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.Configure<DiscordBotOptions>(builder.Configuration.GetSection("Discord"));

var deploymentPath = AppContext.BaseDirectory;
var logDirectory = Path.Combine(deploymentPath, "logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}
var logFilePath = Path.Combine(logDirectory, "log-.log"); 

var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        logFilePath, 
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(loggerConfig, dispose: true);

builder.Services.AddSingleton(new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.All,
    FormatUsersInBidirectionalUnicode = false,
    // Add GatewayIntents.GuildMembers to the GatewayIntents and change this to true if you want to download all users on startup
    AlwaysDownloadUsers = true,
    LogGatewayIntentWarnings = false,
    LogLevel = LogSeverity.Info
});

builder.Services.AddSingleton<EmbedHelper>();

builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<IRestClientProvider>(x => x.GetRequiredService<DiscordSocketClient>());

builder.Services.AddSingleton(new InteractionServiceConfig
{
    LogLevel = LogSeverity.Info,
    UseCompiledLambda = true
});

builder.Services.AddSingleton<InteractionService>();

builder.Services.AddHostedService<DiscordBotService>();
builder.Services.AddHostedService<InteractionHandler>();


var host = builder.Build();
await host.RunAsync();
