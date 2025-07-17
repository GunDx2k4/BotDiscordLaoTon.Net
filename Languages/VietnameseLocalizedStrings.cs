using System;
using Discord;

namespace BotDiscordLaoTon.Net.Languages;

public class VietnameseLocalizedStrings : ILocalizedStrings
{
    public string ERROR => "LỖI";
    public string ResponseSlashCommandTest => "Kiểm tra bot ...";
    public string ResponseSlashCommandSetLanguage => "Đặt ngôn ngữ {0} cho máy chủ {1} đã thành công !";
    public string SuccessfullySlashCommandCreateRole => "Tạo vai trò {0} thành công!";
    public string FailedSlashCommandCreateRole => "Tạo vai trò {0} không thành công!";
    public string ErrorCommandMustBeInGuildChannel => $"Lệnh phải được sử dụng trong {Format.Bold(Format.Code("một kênh của máy chủ."))}";
    public string ErrorBotRequiresGuildPermission => "Bot yêu cầu quyền máy chủ {0}.";
    public string ErrorBotRequiresChannelPermission => "Bot yêu cầu quyền kênh {0}.";
    public string ErrorUserRequiresGuildPermission => "Người dùng yêu cầu quyền máy chủ {0}.";
    public string ErrorUserRequiresChannelPermission => "Người dùng yêu cầu quyền kênh {0}.";
}
