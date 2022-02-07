using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Bot.Identities;

public class TokenIdentity : Identity
{
	private readonly bool _isValid;

	public TokenIdentity(string token, IServiceProvider serviceProvider, ApiToken dbToken) : base(token,
		serviceProvider)
	{
		if (dbToken == null) return;

		using var hmac = new HMACSHA512(dbToken.TokenSalt);
		var generatedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
		var valid = generatedHash.Length != 0;

		for (var i = 0; i < generatedHash.Length; i++)
			if (generatedHash[i] != dbToken.TokenHash[i])
				valid = false;

		if (dbToken.ValidUntil <= DateTime.UtcNow)
			valid = false;

		_isValid = valid;

		CurrentUser = DiscordRest.GetCurrentBotInfo();
		CurrentUserGuilds = new List<UserGuild>();
	}

	public static string TryGetKey(HttpContext context)
	{
		if (context is null) return string.Empty;

		return context.Request.Headers.ContainsKey("Authorization") ? "/api/" + context.Request.Headers["Authorization"] : string.Empty;
	}

	public static async Task<Identity> TryMakeIdentity(HttpContext context, IServiceProvider services)
	{
		var key = TryGetKey(context);

		if (string.IsNullOrEmpty(key)) return null;

		var logger = services.GetRequiredService<ILogger<TokenIdentity>>();

		logger.LogInformation("Registering new TokenIdentity.");

		string fullToken = context.Request.Headers["Authorization"];

		var token = string.Empty;

		try
		{
			// Exclude "Bearer" prefix.
			token = fullToken.Split(' ')[1];
		}
		catch (Exception e)
		{
			logger.LogError("Error while parsing token: " + e.Message);
		}

		ApiToken registeredToken = null;

		try
		{
			using var scope = services.CreateScope();
			registeredToken = await scope.ServiceProvider.GetRequiredService<TokenRepository>().GetToken();
		}
		catch (ResourceNotFoundException)
		{
		}

		return new TokenIdentity(token, services, registeredToken);
	}

	public virtual bool IsAuthorized()
	{
		return _isValid;
	}

	public override bool IsOnGuild(ulong guildId)
	{
		return IsAuthorized();
	}

	public override Task<bool> HasAdminRoleOnGuild(ulong guildId)
	{
		return Task.FromResult(IsAuthorized());
	}

	public override Task<bool> HasModRoleOrHigherOnGuild(ulong guildId)
	{
		return Task.FromResult(IsAuthorized());
	}

	public override Task<bool> IsSiteAdmin()
	{
		return Task.FromResult(IsAuthorized());
	}
}