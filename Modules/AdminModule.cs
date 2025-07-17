using BotDiscordLaoTon.Net.Attributes;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Guilds;
using BotDiscordLaoTon.Net.Helpers;
using BotDiscordLaoTon.Net.Languages;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules;

[BotPermission(GuildPermission.Administrator)]
[UserPermission(GuildPermission.Administrator)]
public class AdminModule(ILogger<AdminModule> logger, EmbedHelper embedHelper) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("set-language", "Set language bot in guild")]
    public async Task SetLanguageAsnyc(Language language)
    {
        GuildManager.GuildConfigurations[Context.Guild.Id].LoadLanguage(language);
        var localizedStrings = GuildManager.GuildConfigurations[Context.Guild.Id].LocalizedStrings;
        logger.LogInformation("{User} Set new lang [{Lang}] in {Guild}", Context.User.ToStringDebug(), language, Context.Guild.ToStringDebug());
        await RespondAsync(embed: embedHelper.SuccessEmbedBuilder(string.Format(localizedStrings.ResponseSlashCommandSetLanguage, Format.Bold(language.ToString()), Format.Bold(Context.Guild.Name))).Build());
    }


    [SlashCommand("test", "Test bot ...")]
    public async Task TestCommandAsync()
    {
        var localizedStrings = GuildManager.GuildConfigurations[Context.Guild.Id].LocalizedStrings;
        logger.LogInformation("{User} Test command in {Guild}", Context.User.ToStringDebug(), Context.Guild.ToStringDebug());
        await RespondAsync(embed: embedHelper.BasicEmbedBuilder(localizedStrings.ResponseSlashCommandTest).Build());
    }

    [Group("create", "Group create")]
    public class CreateGroupModule(ILogger<CreateGroupModule> logger, EmbedHelper embedHelper) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("role", "Create a new role with default color Green.")]
        public async Task CreateRoleAsync([Summary(description: "Name role")] string roleName,
                                          [Summary(description:"Display role members separately")]bool isHoisted = true,
                                          [Summary(description: "Allow anyone to @mention")] bool isMentionable = false,
                                          [Summary(description: "Hex code color (Ex: #FFFFFF or 0xFFFFFF or FFFFFF)")] Color? roleColor = null)
        {
            await DeferAsync();
            var role = await Context.Guild.CreateRoleAsync(name: roleName, color: roleColor ?? Color.Green, isHoisted: isHoisted, isMentionable: isMentionable);
            
            var localizedStrings = GuildManager.GuildConfigurations[Context.Guild.Id].LocalizedStrings;
            if (role != null)
            {
                logger.LogInformation("{User} Create new Role {Role} in {Guild}", Context.User.ToStringDebug(), role.ToStringDebug(), Context.Guild.ToStringDebug());
                await FollowupAsync(embed: embedHelper.SuccessEmbedBuilder(string.Format(localizedStrings.SuccessfullySlashCommandCreateRole, Format.Bold(roleName))).Build());
            }
            else
            {
                await FollowupAsync(embed: embedHelper.ErrorEmbedBuilder(string.Format(localizedStrings.FailedSlashCommandCreateRole, Format.Bold(roleName))).Build());
            }
        }
    }


}
