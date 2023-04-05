using Bot.Abstractions;
using Bot.Data;
using Bot.Services;
using Discord;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Extensions;
using Punishments.Models;

namespace Punishments.Abstractions;

public class PunishmentCommand<T> : Command<T>
{
    public ModCaseRepository ModCaseRepository { get; set; }
    public SettingsRepository SettingsRepository { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    public DiscordRest DiscordRest { get; set; }

    public override async Task BeforeCommandExecute()
    {
        await Context.Interaction.DeferAsync(!GuildConfig.StaffChannels.Contains(Context.Channel.Id));

        ModCaseRepository.AsUser(Identity);
    }

    public async Task RunModCase(ModCase modCase)
    {
        try
        {
            var (_, result, finalWarned) =
                await ModCaseRepository.CreateModCase(modCase);

            if (finalWarned && modCase.PunishmentType != PunishmentType.FinalWarn)
            {
                var textChannel = Context.Guild.GetTextChannel(GuildConfig.StaffAnnouncements);

                await Context.Interaction.ModifyOriginalResponseAsync(msg =>
                    msg.Content =
                        $"Check {textChannel.Mention} for this modlog, as this user is on their final warning."
                );
            }
            else
            {
                var (embed, buttons) = await modCase.CreateNewModCaseEmbed(GuildConfig,
                    await SettingsRepository.GetAppSettings(), result, DiscordRest, ServiceProvider);

                await Context.Interaction.ModifyOriginalResponseAsync(msg =>
                {
                    msg.Embed = embed.Build();
                    msg.Components = buttons.Build();
                });
            }
        }
        catch (Exception e)
        {
            await Context.Interaction.FollowupAsync(embed: new EmbedBuilder()
                .WithTitle("Failed to complete operation")
                .WithDescription(e.Message)
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build());
            throw;
        }
    }
}
