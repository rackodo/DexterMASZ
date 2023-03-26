using Discord;
using Discord.Interactions;
using Lavalink4NET;
using Microsoft.Extensions.DependencyInjection;
using Music.Data;
using Music.Extensions;

namespace Music.Attributes;

public class QueueNotEmptyAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        ICommandInfo commandInfo, IServiceProvider services)
    {
        var audio = services.GetRequiredService<IAudioService>();
        var startRepository = services.GetRequiredService<StartRepository>();
        var player = await audio.EnsureConnected(context, startRepository);

        return player.Queue.IsEmpty ?
            PreconditionResult.FromError("Queue must not be empty before running this command!") :
            PreconditionResult.FromSuccess();
    }
}
