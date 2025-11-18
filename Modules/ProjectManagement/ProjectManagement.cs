using System;
using Discord;
using Discord.Commands;

namespace BotDiscordLaoTon.Net.Modules.ProjectManagement;

[RequireBotPermission(GuildPermission.Administrator)]
[RequireUserPermission(GuildPermission.ManageChannels)]
public class ProjectManagementRequire(ILogger<ProjectManagementRequire> logger) : InteractionModuleLogging(logger)
{

}
