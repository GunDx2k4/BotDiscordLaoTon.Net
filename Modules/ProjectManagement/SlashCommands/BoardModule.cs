using System;
using BotDiscordLaoTon.Net.Extensions;
using BotDiscordLaoTon.Net.Models;
using BotDiscordLaoTon.Net.Repositories;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.ProjectManagement.SlashCommands;

[Group("board", "Commands for managing boards")]
public class BoardModule(ILogger<BoardModule> logger, IBoardRepository boardRepository) : ProjectManagementRequire(logger)
{
    [SlashCommand("create", "Create a new board")]
    public async Task CreateBoardAsync([Summary(description: "The name of the board")] string name, [Summary(description: "Override existing board if it exists")] bool isOverride = false)
    {
        await DeferAsync(ephemeral: false);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
        {
            await FollowupAsync("Board name must be between 1 and 100 characters.");
            return;
        }

        if (isOverride)
        {
            var (memberRole, category, infoChannel, boardChannel) = await Context.GetOrCreateBoardResourcesAsync(name);

            var oldBoard = await boardRepository.GetBoardByCategoryChannelIdAsync(category.Id);
            if (oldBoard != null)
            {
                oldBoard.Name = name;
                oldBoard.MemberRoleId = memberRole.Id;
                oldBoard.InfoChannelId = infoChannel.Id;
                oldBoard.BoardChannelId = boardChannel.Id;

                if (!await boardRepository.UpdateBoardAsync(oldBoard))
                {
                    await boardRepository.AddBoardAsync(new Board
                    {
                        Name = name,
                        CategoryChannelId = category.Id,
                        GuildId = Context.Guild.Id,
                        OwnerId = Context.User.Id,
                        MemberRoleId = memberRole.Id,
                        InfoChannelId = infoChannel.Id,
                        BoardChannelId = boardChannel.Id
                    });
                }
                var embedBuilder = new EmbedBuilder
                {
                    Title = "Board Updated",
                    Description = $"The board {Format.Bold(name)} has been updated successfully.",
                    Fields =
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Category Channel",
                            Value = category.Name,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Info Channel",
                            Value = infoChannel.Mention,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Board Channel",
                            Value = boardChannel.Mention,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Member Role",
                            Value = memberRole.Mention,
                            IsInline = true
                        }
                    },
                };
                await FollowupAsync($"Board {Format.Bold(name)} already exists. Resources have been updated.", embed: embedBuilder.BuildSuccessEmbed());
                return;
            }

            await boardRepository.AddBoardAsync(new Board
            {
                Name = name,
                CategoryChannelId = category.Id,
                GuildId = Context.Guild.Id,
                OwnerId = Context.User.Id,
                MemberRoleId = memberRole.Id,
                InfoChannelId = infoChannel.Id,
                BoardChannelId = boardChannel.Id
            });
            await FollowupAsync($"Board {Format.Bold(name)} has been created successfully.");
        }
        else
        {
            var (memberRole, category, infoChannel, boardChannel) = await Context.CreateBoardResourcesAsync(name);

            var embedBuilder = new EmbedBuilder
            {
                Title = "Board Created",
                Description = $"The board {Format.Bold(name)} has been created successfully.",
                Fields =
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Category Channel",
                        Value = category.Name,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Info Channel",
                        Value = infoChannel.Mention,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Board Channel",
                        Value = boardChannel.Mention,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Member Role",
                        Value = memberRole.Mention,
                        IsInline = true
                    }
                },
            };

            await FollowupAsync($"Board {Format.Bold(name)} has been created or updated successfully.", embed: embedBuilder.BuildSuccessEmbed());

            var board = new Board
            {
                Name = name,
                CategoryChannelId = category.Id,
                GuildId = Context.Guild.Id,
                OwnerId = Context.User.Id,
                MemberRoleId = memberRole.Id,
                InfoChannelId = infoChannel.Id,
                BoardChannelId = boardChannel.Id
            };

            await boardRepository.AddBoardAsync(board);
        }


    }

    [SlashCommand("list", "List all existing boards")]
    public async Task ListBoardsAsync()
    {
        await DeferAsync();

        var boards = await boardRepository.GetBoardsByGuildIdAsync(Context.Guild.Id);
        if (boards.Count == 0)
        {
            await FollowupAsync("No boards found in this server.");
            return;
        }

        foreach (var board in boards)
        {
            try
            {
                var owner = await Context.Guild.GetUserAsync(board.OwnerId);
                var memberRole = await Context.Guild.GetRoleAsync(board.MemberRoleId);
                var boardChannel = await Context.Guild.GetChannelAsync(board.BoardChannelId) as ITextChannel;
                var embedBuilder = new EmbedBuilder
                {
                    Title = board.Name,
                    Fields =
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Owner",
                        Value = owner != null ? owner.Mention : "Unknown",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Member Role",
                        Value = memberRole != null ? memberRole.Mention : "Unknown",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Board Channel",
                        Value = boardChannel != null ? boardChannel.Mention : "Unknown",
                        IsInline = true
                    }
                },
                };

                var deleteButton = new ButtonBuilder
                {
                    Label = "Delete Board",
                    CustomId = $"delete_board_{board.Id}".ToButtonId(),
                    Style = ButtonStyle.Danger
                };

                var component = new ComponentBuilder()
                    .WithButton(deleteButton)
                    .Build();

                await FollowupAsync(embed: embedBuilder.BuildInfoEmbed(), components: component);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error listing board with ID {BoardId}", board.Id);
                await boardRepository.DeleteBoardAsync(board.Id);
                continue;
            }

        }

    }
}
