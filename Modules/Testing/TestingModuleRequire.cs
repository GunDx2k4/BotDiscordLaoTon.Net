using System;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing;

[RequireBotPermission(Discord.GuildPermission.Administrator)]
public class TestingModuleRequire(ILogger<TestingModuleRequire> logger) : InteractionModuleLogging(logger)
{

}
