using Bot.Abstractions;
using Bot.Models;
using Bot.Services;
using Discord.WebSocket;
using Fergun.Interactive;
using Lavalink4NET.Artwork;
using Lavalink4NET.Clients;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Extensions;
using Lavalink4NET.InactivityTracking;
using Lavalink4NET.InactivityTracking.Extensions;
using Lavalink4NET.InactivityTracking.Trackers.Idle;
using Lavalink4NET.Lyrics;
using Lavalink4NET.Tracking;
using Microsoft.Extensions.DependencyInjection;

namespace Music;

public class MusicModule : Module
{
    public const string Host = "lavalink.usfurries.com";
    public const int Port = 2333;
    public const string Pass = "youshallnotpass";

    public override string[] Contributors { get; } = ["Swyreee", "Ferox"];

    public override void AddServices(IServiceCollection services, CachedServices cachedServices,
        AppSettings appSettings) =>
        services
            .AddSingleton(new InteractiveConfig { DefaultTimeout = TimeSpan.FromMinutes(5) })
            .AddSingleton<InteractiveService>()

            .AddSingleton<LyricsOptions>()
            .AddSingleton<LyricsService>()

            .AddSingleton<ArtworkService>()

            .ConfigureInactivityTracking(options =>
            {
                options.DefaultPollInterval = TimeSpan.FromMinutes(5);
                options.DefaultTimeout = TimeSpan.FromMinutes(5);
                options.UseDefaultTrackers = true;
            })
            .Configure<IdleInactivityTrackerOptions>(config => config.Timeout = TimeSpan.FromSeconds(10))
            .AddInactivityTracking()
            .AddSingleton<InactivityTrackingService>()

            .ConfigureLavalink(config =>
            {
                config.BaseAddress = new Uri($"http://{Host}:{Port}");
                config.WebSocketUri = new Uri($"ws://{Host}:{Port}/v4/websocket");
                config.ReadyTimeout = TimeSpan.FromSeconds(10);
                config.Passphrase = Pass;
            })
            .AddLavalink();
}
