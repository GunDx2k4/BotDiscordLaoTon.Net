using System;

namespace BotDiscordLaoTon.Net.Languages;

public interface ILocalizedStrings
{
    string ERROR { get; }
    string ErrorCommandMustBeInGuildChannel { get; }
    string ResponseSlashCommandTest { get; }
    string ResponseSlashCommandSetLanguage { get; }
    string SuccessfullySlashCommandCreateRole { get; }
    string FailedSlashCommandCreateRole { get; }
    string ErrorBotRequiresGuildPermission { get; }
    string ErrorBotRequiresChannelPermission { get; }
    string ErrorUserRequiresGuildPermission { get; }
    string ErrorUserRequiresChannelPermission { get; }
    
}
