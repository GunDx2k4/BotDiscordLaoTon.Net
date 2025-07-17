using System;
using BotDiscordLaoTon.Net.Languages;

namespace BotDiscordLaoTon.Net.Guilds;

public class GuildConfiguration
{
    public string Prefix { get; set; } = "!";

    public ILocalizedStrings LocalizedStrings { get; set; } = new EnglishLocalizedStrings();
    
    public void LoadLanguage(Language language = Language.English)
    {
        switch (language)
        {
            case Language.English:
                LocalizedStrings = new EnglishLocalizedStrings();
                break;
            case Language.Vietnamese:
                LocalizedStrings = new VietnameseLocalizedStrings();
                break;
        }
    }
}
