using System;
using System.ComponentModel;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing.SelectMenus;

public enum TestingMenuOptions
{
    [Description("Displays information about the bot")]
    ShowBotInfo,

    [Description("Displays information about the server")]
    ShowServerInfo,

    [Description("Displays information about the user")]
    ShowUserInfo,

    [Description("Runs diagnostic checks on the bot")]
    RunDiagnostics
}

public class TestingSelectMenuModule(ILogger<TestingSelectMenuModule> logger) : TestingModuleRequire(logger)
{
    [ComponentInteraction($"{Constants.SelectMenuCustomIdPrefix}_testing")]
    public async Task HandleTestingMenuSelectionAsync(string[] selectedValues)
    {
        if (selectedValues.Length == 0)
        {
            await RespondAsync("No option selected.", ephemeral: true);
            return;
        }

        var selectedOption = selectedValues[0];
        switch (selectedOption)
        {
            case "show_bot_info":
                await RespondAsync("Bot Info: [Details about the bot]", ephemeral: true);
                break;
            case "show_server_info":
                await RespondAsync("Server Info: [Details about the server]", ephemeral: true);
                break;
            case "show_user_info":
                await RespondAsync("User Info: [Details about the user]", ephemeral: true);
                break;
            case "run_diagnostics":
                await RespondAsync("Running diagnostics... [Diagnostic results]", ephemeral: true);
                break;
            default:
                await RespondAsync("Unknown option selected.", ephemeral: true);
                break;
        }
    }

}
