using Bot.Abstractions;
using Bot.Models;
using Bot.Services;
using Discord.WebSocket;
using Fergun.Interactive;
using Lavalink4NET;
using Lavalink4NET.Artwork;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Logging.Microsoft;
using Lavalink4NET.Lyrics;
using Lavalink4NET.MemoryCache;
using Lavalink4NET.Tracking;
using Microsoft.Extensions.DependencyInjection;

namespace Music;

public class MusicModule : Module
{
    public override string[] Contributors { get; } = { "Swyreee", "Ferox" };

    public override void AddServices(IServiceCollection services, CachedServices cachedServices,
        AppSettings appSettings) =>
        services.AddSingleton(x => new InteractiveService(x.GetRequiredService<DiscordShardedClient>()))
            .AddSingleton<ILavalinkCache, LavalinkCache>()
            .AddSingleton<IAudioService, LavalinkNode>()
            .AddSingleton<LyricsOptions>()
            .AddSingleton<LyricsService>()
            .AddSingleton<ArtworkService>()
            .AddSingleton(new InactivityTrackingOptions
            {
                PollInterval = TimeSpan.FromMinutes(5), DisconnectDelay = TimeSpan.Zero, TrackInactivity = true
            })
            .AddSingleton<InactivityTrackingService>()
            .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>(x =>
                new DiscordClientWrapper(x.GetRequiredService<DiscordShardedClient>()))
            .AddMicrosoftExtensionsLavalinkLogging()
            .AddSingleton(new LavalinkNodeOptions
            {
                DebugPayloads = true,
                DisconnectOnStop = false
            });

    public override void PostBuild(IServiceProvider services, CachedServices cachedServices) =>
        services.GetRequiredService<LavalinkNode>().UseSponsorBlock();
}
