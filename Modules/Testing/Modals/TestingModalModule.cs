using System;
using Discord;
using Discord.Interactions;

namespace BotDiscordLaoTon.Net.Modules.Testing.Modals;

public class TestingModal : IModal
{
    public string Title => "Testing Modal";

    [InputLabel("Enter testing information")]
    [ModalTextInput("testing_input", TextInputStyle.Paragraph, placeholder: "Type here...", maxLength: 4000)]
    public string TestingInput { get; set; } = string.Empty;
}

public class TestingModalModule(ILogger<TestingModalModule> logger) : TestingModuleRequire(logger)
{
    [ModalInteraction($"{Constants.ModalCustomIdPrefix}_testing")]
    public async Task HandleTestingModalAsync(TestingModal modal)
    {
        string testingInfo = modal.TestingInput;
        // Process the testing information as needed
        await RespondAsync($"Received testing information:\n{testingInfo}", ephemeral: true);
    }
}
