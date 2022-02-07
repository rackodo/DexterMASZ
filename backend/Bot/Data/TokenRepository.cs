using Bot.Abstractions;
using Bot.DTOs;
using Bot.Dynamics;
using Bot.Events;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bot.Data;

public class TokenRepository : Repository, AddAdminStats
{
	private readonly BotEventHandler _eventHandler;
	private readonly SettingsRepository _settingsRepo;
	private readonly BotDatabase _botDatabase;

	public TokenRepository(BotDatabase botDatabase, BotEventHandler eventHandler,
		SettingsRepository settingsRepo, DiscordRest discordRest) : base(discordRest)
	{
		_botDatabase = botDatabase;
		_eventHandler = eventHandler;
		_settingsRepo = settingsRepo;

		_settingsRepo.AsUser(Identity);
	}

	public async Task AddAdminStatistics(dynamic adminStats)
	{
		adminStats.apiTokens = await CountTokens();
	}

	public async Task<ApiToken> GetToken()
	{
		var apiToken = await _botDatabase.GetApiToken();

		if (apiToken == null)
			throw new ResourceNotFoundException("Token does not exist.");

		return apiToken;
	}

	public async Task<int> CountTokens()
	{
		return await _botDatabase.CountAllApiTokens();
	}

	public async Task<TokenCreatedDto> RegisterToken(string name)
	{
		var token = await GenerateToken(name);

		ApiToken apiToken = new();

		using var hmac = new HMACSHA512();
		apiToken.TokenHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
		apiToken.TokenSalt = hmac.Key;
		apiToken.CreatedAt = DateTime.UtcNow;
		apiToken.ValidUntil = DateTime.UtcNow.AddYears(1);
		apiToken.Name = name;

		await _botDatabase.SaveToken(apiToken);

		_eventHandler.TokenCreatedEvent.Invoke(apiToken);

		return new TokenCreatedDto
		{
			Token = token,
			Id = apiToken.Id
		};
	}

	public async Task DeleteToken()
	{
		var apiToken = await GetToken();

		await _botDatabase.DeleteToken(apiToken);

		_eventHandler.TokenDeletedEvent.Invoke(apiToken);
	}

	private async Task<string> GenerateToken(string name)
	{
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.NameId, name)
		};

		var settings = await _settingsRepo.GetAppSettings();

		var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.DiscordBotToken)),
			SecurityAlgorithms.HmacSha512Signature);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.Now.AddYears(1),
			SigningCredentials = credentials
		};

		var tokenHandler = new JwtSecurityTokenHandler();

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}