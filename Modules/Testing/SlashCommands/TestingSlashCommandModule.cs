using System;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Modules.Testing.Modals;
using BotDiscordLaoTon.Net.Modules.Testing.SelectMenus;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing.SlashCommands;

public class TestingSlashCommandModule(ILogger<TestingSlashCommandModule> logger) : TestingModuleRequire(logger)
{
    [SlashCommand("ping", "Check the bot's responsiveness")]
    public async Task PingAsync()
    {
        await RespondAsync("Pong!", ephemeral: true);
    }

    [SlashCommand("menu", "Display a testing menu")]
    public async Task TestingMenuAsync()
    {
        var embed = new EmbedBuilder()
            .WithTitle("Testing Menu")
            .WithDescription("Select an option from the menu below to perform a testing action.")
            .BuildInfoEmbed();

        var selectMenuBuilder = new SelectMenuBuilder
        {
            CustomId = "testing".ToSelectMenuId(),
            Placeholder = "Choose a testing action"
        };
        
        selectMenuBuilder.AddOption<TestingMenuOptions>();

        var components = new ComponentBuilder()
            .WithSelectMenu(selectMenuBuilder)
            .Build();

        await RespondAsync(embed: embed, components: components, ephemeral: true);
    }

    [SlashCommand("modal", "Open a testing modal")]
    public async Task TestingModalAsync()
    {
        await Context.Interaction.RespondWithModalAsync<TestingModal>("testing".ToModalId());
    }

    [SlashCommand("button", "Test a testing button")]
    public async Task TestingButtonAsync()
    {
        var components = new ComponentBuilder()
            .WithButton("Testing Button", "testing".ToButtonId(), ButtonStyle.Primary)
            .Build();
        await RespondAsync("Testing button test.", components: components, ephemeral: true);
    }
}
