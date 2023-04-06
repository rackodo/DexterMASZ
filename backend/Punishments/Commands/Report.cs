using Bot.Abstractions;
using Bot.Extensions;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Punishments.Extensions;
using Punishments.Translators;

namespace Punishments.Commands;

public class Report : Command<Report>
{
    public DiscordSocketClient Client { get; set; }
    public IServiceProvider Services { get; set; }

    public override Task BeforeCommandExecute() => DeferAsync(true);

    [MessageCommand("Report to moderators")]
    public async Task ReportCommand(IMessage msg)
    {
        try
        {
            var embed = await msg.CreateReportEmbed(Context.User, Services);

            await Client.SendEmbed(GuildConfig.GuildId, GuildConfig.StaffAnnouncements, embed);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to send internal notification to moderators for report command.");
            await RespondAsync(Translator.Get<PunishmentTranslator>().ReportFailed());
            return;
        }

        await RespondAsync(Translator.Get<PunishmentTranslator>().ReportSent());
    }
}
