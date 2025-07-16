using BotDiscordLaoTon.Net.Attributes;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Helpers;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules;

[BotPermission(GuildPermission.Administrator)]
[UserPermission(GuildPermission.Administrator)]
public class AdminModule(ILogger<AdminModule> logger, EmbedHelper embedHelper) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("test", "Lệnh kiểm tra bot ...")]
    public async Task TestCommandAsync()
    {
        logger.LogInformation("{User} Test command", Context.User);
        await RespondAsync(embed: embedHelper.BasicEmbedBuilder("Test thôi mà cu").Build());
    }

    [Group("create", "Nhóm lệnh khởi tạo")]
    public class CreateGroupModule(ILogger<CreateGroupModule> logger, EmbedHelper embedHelper) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("role", "Tạo mới vai trò màu mặc định là Green")]
        public async Task CreateRoleAsync([Summary(description: "Tên vai trò")] string roleName,
                                          [Summary(description:"Cho phép hiển thị vai trò riêng biệt")]bool isHoisted = true,
                                          [Summary(description: "Cho phép mọi người @mention")] bool isMentionable = false,
                                          [Summary(description: "Mã màu vai trò (VD: #FFFFFF hoặc 0xFFFFFF, FFFFFF)")] Color? roleColor = null)
        {
            await DeferAsync();
            var role = await Context.Guild.CreateRoleAsync(name: roleName, color: roleColor ?? Color.Green, isHoisted: isHoisted, isMentionable: isMentionable);

            if (role != null)
            {
                logger.LogInformation("{User} Create new Role {Role} in {Guild}", Context.User.ToStringDebug(), role.ToStringDebug(), Context.Guild.ToStringDebug());
                await FollowupAsync(embed: embedHelper.SuccessEmbedBuilder($"Tạo vai trò {Format.Bold(role.Mention)} thành công !").Build());
            }
            else
            {
                await FollowupAsync(embed: embedHelper.ErrorEmbedBuilder($"Tạo vai trò {Format.Bold(roleName)} không thành công !").Build());
            }
        }
    }


}
