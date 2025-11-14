using BotDiscordLaoTon.Net;
using BotDiscordLaoTon.Net.Data;
using BotDiscordLaoTon.Net.Options;
using BotDiscordLaoTon.Net.Services;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

builder.Services.AddSingleton(new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.All,
    FormatUsersInBidirectionalUnicode = false,
    AlwaysDownloadUsers = true,
    LogGatewayIntentWarnings = false,
    LogLevel = LogSeverity.Info
});

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

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "BotDiscordLaoTon.Net";
});

var host = builder.Build();

await host.RunAsync();
