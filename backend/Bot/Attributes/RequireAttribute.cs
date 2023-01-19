using Bot.Data;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Attributes;

public class RequireAttribute : PreconditionAttribute
{
    private readonly RequireCheck[] _checks;

    public RequireAttribute(params RequireCheck[] checks) => _checks = checks;

    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        ICommandInfo commandInfo, IServiceProvider services)
    {
        var identity = await services.GetRequiredService<IdentityManager>().GetIdentity(context.User);

        try
        {
            await services.GetRequiredService<GuildConfigRepository>().GetGuildConfig(context.Guild.Id);
        }
        catch (ResourceNotFoundException)
        {
            throw new UnregisteredGuildException(context.Guild.Id);
        }

        if (await identity.IsSiteAdmin())
            return PreconditionResult.FromSuccess();

        foreach (var check in _checks)
        {
            var canRun = check switch
            {
                RequireCheck.GuildUser => identity.IsOnGuild(context.Guild.Id),
                RequireCheck.GuildModerator => await identity.HasModRoleOrHigherOnGuild(context.Guild.Id),
                RequireCheck.GuildAdmin => await identity.HasAdminRoleOnGuild(context.Guild.Id),
                RequireCheck.GuildStrictModeMute => identity.HasPermission(GuildPermission.MuteMembers,
                    context.Guild.Id),
                RequireCheck.GuildStrictModeKick => identity.HasPermission(GuildPermission.KickMembers,
                    context.Guild.Id),
                RequireCheck.GuildStrictModeBan => identity.HasPermission(GuildPermission.BanMembers, context.Guild.Id),
                RequireCheck.SiteAdmin => throw new UnauthorizedException("Only site admins are allowed."),
                _ => throw new NotImplementedException()
            };

            if (!canRun)
                throw new UnauthorizedException($"You are not allowed to do that, missing {check.Humanize()}.");
        }

        return PreconditionResult.FromSuccess();
    }
}
