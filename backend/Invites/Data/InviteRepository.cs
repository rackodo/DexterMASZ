using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Invites.Events;
using Invites.Models;
using Invites.Translators;
using System.Text;
using Utilities.Dynamics;

namespace Invites.Data;

public class InviteRepository : Repository,
    IAddAdminStats, IAddGuildStats, IAddNetworks, IWhoIsResults, IDeleteGuildData
{
    private readonly DiscordRest _discordRest;
    private readonly InviteEventHandler _eventHandler;
    private readonly InviteDatabase _userInviteDatabase;

    public InviteRepository(InviteDatabase userInviteDatabase, InviteEventHandler eventHandler, DiscordRest discordRest)
        : base(discordRest)
    {
        _userInviteDatabase = userInviteDatabase;
        _eventHandler = eventHandler;
        _discordRest = discordRest;
    }

    public async Task AddAdminStatistics(dynamic adminStats) => adminStats.trackedInvites = await CountInvites();

    public async Task AddGuildStatistics(dynamic stats, ulong guildId) =>
        stats.trackedInvites = await CountInvitesForGuild(guildId);

    public async Task AddNetworkData(dynamic network, List<string> modGuilds, ulong userId)
    {
        var invited = new List<UserInviteExpanded>();

        foreach (var invite in (await GetInvitedForUser(userId)).Where(invite =>
                     modGuilds.Contains(invite.GuildId.ToString())))
        {
            if (invite.InviteIssuerId == 0)
                continue;

            invited.Add(new UserInviteExpanded(
                invite,
                await _discordRest.FetchUserInfo(invite.JoinedUserId, true),
                await _discordRest.FetchUserInfo(invite.InviteIssuerId, true)
            ));
        }

        var invitedBy = new List<UserInviteExpanded>();
        foreach (var invite in (await GetUsedInvitesForUser(userId)).Where(invite =>
                     modGuilds.Contains(invite.GuildId.ToString())))
        {
            if (invite.InviteIssuerId == 0)
                continue;

            invitedBy.Add(new UserInviteExpanded(
                invite,
                await _discordRest.FetchUserInfo(invite.JoinedUserId, true),
                await _discordRest.FetchUserInfo(invite.InviteIssuerId, true)
            ));
        }

        network.invited = invited;
        network.invitedBy = invitedBy;
    }

    public async Task DeleteGuildData(ulong guildId) => await _userInviteDatabase.DeleteInviteHistoryByGuild(guildId);

    public async Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
        Translation translator)
    {
        var invites = await GetUsedInvitesForUserAndGuild(user.Id, context.Guild.Id);
        var filteredInvites = invites.OrderByDescending(x => x.JoinedAt).ToList();

        if (user is { JoinedAt: { } })
            filteredInvites = filteredInvites.FindAll(x => x.JoinedAt >= user.JoinedAt.Value.UtcDateTime);

        if (user.JoinedAt != null)
            embed.AddField(translator.Get<BotTranslator>().Joined(), user.JoinedAt.Value.DateTime.ToDiscordTs());

        if (filteredInvites.Count > 0)
        {
            StringBuilder joinedInfo = new();

            foreach (var usedInvite in filteredInvites)
            {
                joinedInfo.AppendLine($"`{usedInvite.UsedInvite}`.");

                if (usedInvite.InviteIssuerId != 0)
                    joinedInfo.AppendLine(translator.Get<InviteTranslator>().ByUser(usedInvite.InviteIssuerId));
            }

            embed.AddField(translator.Get<InviteTranslator>().UsedInvite(), joinedInfo);
        }
    }

    public async Task<int> CountInvites() => await _userInviteDatabase.CountTrackedInvites();

    public async Task<int> CountInvitesForGuild(ulong guildId) =>
        await _userInviteDatabase.CountTrackedInvitesForGuild(guildId);

    public async Task<List<UserInvite>> GetInvitedForUser(ulong userId) =>
        await _userInviteDatabase.GetInvitedUsersByUser(userId);

    public async Task<List<UserInvite>> GetUsedInvitesForUserAndGuild(ulong userId, ulong guildId) =>
        await _userInviteDatabase.GetUsedInvitesByUserAndGuild(userId, guildId);

    public async Task<List<UserInvite>> GetUsedInvitesForUser(ulong userId) =>
        await _userInviteDatabase.GetUsedInvitesByUser(userId);

    public async Task<List<UserInvite>> GetInvitesByCode(string code) =>
        await _userInviteDatabase.GetInvitesByCode(code);

    public async Task<UserInvite> CreateInvite(UserInvite invite)
    {
        await _userInviteDatabase.SaveInvite(invite);

        _eventHandler.InviteUsageRegisteredEvent.Invoke(invite);

        return invite;
    }
}
