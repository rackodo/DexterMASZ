using System.Net;
using System.Text;
using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Invites.Data;
using Invites.Translators;

namespace Invites.Commands;

public class Track : Command<Track>
{
    public InviteRepository InviteRepository { get; set; }
    public DiscordRest DiscordRest { get; set; }

    [Require(RequireCheck.GuildModerator)]
    [SlashCommand("track", "Track an invite, its creator and its users.")]
    public async Task TrackCommand([Summary("invite", "Either enter the invite code or the url")] string inviteCode)
    {
        InviteRepository.AsUser(Identity);

        await Context.Interaction.RespondAsync("Tracking invite code...");

        if (!inviteCode.ToLower().Search("https://discord.gg/"))
            inviteCode = $"https://discord.gg/{inviteCode}";

        var invites = await InviteRepository.GetInvitesByCode(inviteCode);
        invites = invites.Where(x => x.GuildId == Context.Guild.Id).OrderByDescending(x => x.JoinedAt).ToList();

        DateTime? createdAt = null;
        IUser creator = null;
        int? usages = invites.Count;
        Dictionary<ulong, IUser> invitees = new();

        if (invites.Count > 0)
        {
            createdAt = invites.First().InviteCreatedAt;

            if (invites.First().InviteIssuerId != 0)
                creator = await DiscordRest.FetchUserInfo(invites.First().InviteIssuerId, false);

            const int count = 0;

            foreach (var invite in invites.TakeWhile(_ => count <= 20)
                         .Where(invite => !invitees.ContainsKey(invite.JoinedUserId)))
            {
                invitees.Add(invite.JoinedUserId,
                    await DiscordRest.FetchUserInfo(invite.JoinedUserId, true));
            }
        }
        else
        {
            var code = inviteCode.Split("/").Last();
            try
            {
                var fetchedInvite = await Context.Client.GetInviteAsync(code);
                if (fetchedInvite.GuildId != Context.Guild.Id)
                {
                    await Context.Interaction.ModifyOriginalResponseAsync(message =>
                        message.Content = Translator.Get<InviteTranslator>().InviteNotFromThisGuild());
                    return;
                }

                try
                {
                    usages = fetchedInvite.Uses;
                    creator = await DiscordRest.FetchUserInfo(fetchedInvite.Inviter.Id, false);
                }
                catch (NullReferenceException)
                {
                }
            }
            catch (HttpException e)
            {
                if (e.HttpCode == HttpStatusCode.NotFound)
                    await Context.Interaction.ModifyOriginalResponseAsync(message =>
                        message.Content = Translator.Get<InviteTranslator>().CannotFindInvite());
                else
                    await Context.Interaction.ModifyOriginalResponseAsync(message =>
                        message.Content = Translator.Get<InviteTranslator>().FailedToFetchInvite());
                return;
            }
        }

        EmbedBuilder embed = new();
        embed.WithDescription(inviteCode);

        if (creator != null)
        {
            embed.WithAuthor(creator);

            if (createdAt.HasValue && createdAt.Value != default)
                embed.WithDescription(Translator.Get<InviteTranslator>()
                    .CreatedByAt(inviteCode, creator, createdAt.Value));
            else
                embed.WithDescription(Translator.Get<InviteTranslator>().CreatedBy(inviteCode, creator));
        }
        else if (createdAt.HasValue && createdAt.Value != default)
        {
            embed.WithDescription(Translator.Get<InviteTranslator>().CreatedAt(inviteCode, createdAt.Value));
        }

        StringBuilder usedBy = new();
        foreach (var invite in invites.TakeWhile(_ => usedBy.Length <= 900))
        {
            usedBy.Append("- ");

            if (invitees.ContainsKey(invite.JoinedUserId))
            {
                var user = invitees[invite.JoinedUserId];
                usedBy.Append($"`{user.Username}#{user.Discriminator}` ");
            }

            usedBy.AppendLine($"`{invite.JoinedUserId}` - {invite.JoinedAt.ToDiscordTs()}");
        }

        if (invites.Count == 0)
        {
            usedBy.Clear();
            usedBy.Append(Translator.Get<InviteTranslator>().NotTrackedYet());
        }

        embed.AddField(Translator.Get<InviteTranslator>().UsedBy(usages.GetValueOrDefault()), usedBy.ToString());
        embed.WithFooter($"Invite: {inviteCode}");
        embed.WithCurrentTimestamp();
        embed.WithColor(Color.Gold);

        await Context.Interaction.ModifyOriginalResponseAsync(message =>
        {
            message.Content = "";
            message.Embed = embed.Build();
        });
    }
}