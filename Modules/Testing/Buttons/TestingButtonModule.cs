using System;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing.Buttons;

public class TestingButtonModule(ILogger<TestingButtonModule> logger) : TestingModuleRequire(logger)
{
    [ComponentInteraction($"{Constants.ButtonCustomIdPrefix}_testing")]
    public async Task HandleTestingButtonAsync()
    {
        await RespondAsync("Testing button clicked!", ephemeral: true);
    }
}