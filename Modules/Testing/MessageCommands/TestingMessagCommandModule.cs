using System;
using BotDiscordLaoTon.Net.Extensions;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing.MessageCommands;

public class TestingMessageCommandModule(ILogger<TestingMessageCommandModule> logger) : TestingModuleRequire(logger)
{
    [MessageCommand("Testing Message Command")]
    public async Task TestingMessageCommandAsync(IMessage message)
    {
        await RespondAsync($"You have selected the message with ID {message.ToStringDebug()} for testing.", ephemeral: true);
    }

}
