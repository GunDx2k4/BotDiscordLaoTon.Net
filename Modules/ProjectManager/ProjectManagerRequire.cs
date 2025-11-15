using System;
using Discord.Commands;

namespace BotDiscordLaoTon.Net.Modules.ProjectManager;

[RequireBotPermission(Discord.GuildPermission.Administrator)]
[RequireUserPermission(Discord.GuildPermission.Administrator)]
public class ProjectManagerRequire(ILogger<ProjectManagerRequire> logger) : InteractionModuleLogging(logger)
{

}
