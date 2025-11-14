using System;
using BotDiscordLaoTon.Net.Extensions;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing.UserCommands;

public class TestingUserCommandModule(ILogger<TestingUserCommandModule> logger) : TestingModuleRequire(logger)
{
    [UserCommand("Test")]
    public async Task TestUserCommandAsync(IUser user)
    {
        await RespondAsync($"You have selected {user.ToStringDebug()} for debugging.", ephemeral: true);
    }

}
