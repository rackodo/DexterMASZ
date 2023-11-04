using Bot.Abstractions;
using Discord.Interactions;
using Greeting.Data;
using Greeting.Models;

namespace Greeting.Commands;

public class TempAddGf : Command<TempAddGf>
{
    public GreeterDatabase GreeterDatabase { get; set; }

    [SlashCommand("tempaddgf", "Temp command for adding gf module")]
    public async Task AddGf()
    {
        var greetGate = await GreeterDatabase.GreeterConfigs.FindAsync(Context.Guild.Id);
        if (greetGate != null)
            return;
        GreeterDatabase.GreeterConfigs.Add(new GreetGateModel()
        {
            PunishmentTime = TimeSpan.FromDays(7),
            AllowedGreetChannels = [1163591887102431293, 544891764604141568],
            AllowedGreetRoles = [1167624244390273114, 506206499954098196],
            DisallowedMuteExistence = TimeSpan.FromDays(14),
            DisallowedMuteRoles = [341467344045867009],
            GuildId = Context.Guild.Id,
            LoggingChannel = 536690500959207444
        });
        await GreeterDatabase.SaveChangesAsync();
        await RespondInteraction("Done!");
    }
}
