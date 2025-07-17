using System;
using Discord;

namespace BotDiscordLaoTon.Net.Languages;

public class EnglishLocalizedStrings : ILocalizedStrings
{
    public string ERROR  => "ERROR";
    public string ResponseSlashCommandTest => "Test bot ...";
    public string ResponseSlashCommandSetLanguage => "Language set to {0} for server {1} successfully!";
    public string SuccessfullySlashCommandCreateRole => "Successfully created role {0}!";
    public string FailedSlashCommandCreateRole => "Failed to create role {0}!";
    public string ErrorCommandMustBeInGuildChannel => $"Command must be used in {Format.Bold(Format.Code("a guild channel."))}";
    public string ErrorBotRequiresGuildPermission => "Bot requires guild permission {0}.";
    public string ErrorBotRequiresChannelPermission => "Bot requires channel permission {0}.";
    public string ErrorUserRequiresGuildPermission => "User requires guild permission {0}.";
    public string ErrorUserRequiresChannelPermission => "User requires channel permission {0}.";
}
