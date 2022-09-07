using Bot.Enums;
using Bot.Exceptions;
using Bot.Models;
using Bot.Services;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Abstractions;

public abstract class Identity
{
	protected readonly DiscordRest DiscordRest;
	public readonly IServiceProvider ServiceProvider;

	public readonly string Token;

	protected IUser CurrentUser;
	protected List<UserGuild> CurrentUserGuilds;

	protected Identity(string token, IServiceProvider serviceProvider)
	{
		Token = token;
		ValidUntil = DateTime.UtcNow.AddMinutes(15);

		ServiceProvider = serviceProvider;
		DiscordRest = serviceProvider.GetRequiredService<DiscordRest>();
	}

	public DateTime ValidUntil { get; set; }

	public abstract bool IsOnGuild(ulong guildId);

	public abstract Task<bool> HasAdminRoleOnGuild(ulong guildId);

	public abstract Task<bool> HasModRoleOrHigherOnGuild(ulong guildId);

	public async Task<bool> HasPermission(GuildPermission permission, ulong guildId)
	{
		var guild = DiscordRest.FetchGuildInfo(guildId, CacheBehavior.Default);

		if (CurrentUser is null || guild is null)
			return false;

		var user = await DiscordRest.FetchUserInfo(guildId, CurrentUser.Id, CacheBehavior.Default);

		if (user is null)
			return false;

		return user.Guild.OwnerId == user.Id || user.GuildPermissions.Has(permission);
	}

	public async Task<bool> HasPermission(DiscordPermission permission, ulong guildId)
	{
		using var scope = ServiceProvider.CreateScope();

		if (await IsSiteAdmin())
			return true;

		try
		{
			return permission switch
			{
				DiscordPermission.User => IsOnGuild(guildId),
				DiscordPermission.Moderator => await HasModRoleOrHigherOnGuild(guildId),
				DiscordPermission.Admin => await HasAdminRoleOnGuild(guildId),
				_ => false
			};
		} catch (UnregisteredGuildException)
		{
			var guild = DiscordRest.FetchGuildInfo(guildId, CacheBehavior.Default);
			var user = await guild.GetUserAsync(CurrentUser.Id);

			return permission switch
			{
				DiscordPermission.User => user != null,
				DiscordPermission.Moderator => user.GuildPermissions.Administrator,
				DiscordPermission.Admin => user.GuildPermissions.Administrator,
				_ => false
			};
		}
	}

	public async Task RequireSiteAdmin()
	{
		if (!await IsSiteAdmin())
			throw new UnauthorizedException();
	}

	public async Task RequirePermission(DiscordPermission permission, ulong guildId)
	{
		if (!await HasPermission(permission, guildId))
			throw new UnauthorizedException();
	}

	public abstract Task<bool> IsSiteAdmin();

	public IUser GetCurrentUser()
	{
		if (CurrentUser is null)
			throw new InvalidIdentityException(Token);

		return CurrentUser;
	}

	public List<UserGuild> GetCurrentUserGuilds()
	{
		if (CurrentUserGuilds is null)
			throw new InvalidIdentityException(Token);

		return CurrentUserGuilds;
	}

	public virtual void RemoveGuildMembership(ulong guildId)
	{
		CurrentUserGuilds.RemoveAll(x => x.Id == guildId);
	}

	public virtual void AddGuildMembership(IGuildUser user)
	{
		if (CurrentUserGuilds.All(x => x.Id != user.Guild.Id))
			CurrentUserGuilds.Add(new UserGuild(user));
	}

	public virtual void UpdateGuildMembership(IGuildUser user)
	{
		if (CurrentUserGuilds.All(x => x.Id != user.Guild.Id))
			CurrentUserGuilds.Add(new UserGuild(user));
	}
}