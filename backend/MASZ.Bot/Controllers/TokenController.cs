using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.DTOs;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Identities;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Bot.Controllers;

[Route("api/v1/token")]
public class TokenController : AuthenticatedController
{
	private readonly IdentityManager _identityManager;
	private readonly TokenRepository _tokenRepo;

	public TokenController(IdentityManager identityManager, TokenRepository tokenRepo) :
		base(identityManager, tokenRepo)
	{
		_identityManager = identityManager;
		_tokenRepo = tokenRepo;
	}

	[HttpPost]
	public async Task<IActionResult> CreateToken([FromBody] TokenForCreateDto tokenDto)
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		if (identity is TokenIdentity)
			throw new TokenCannotManageException();

		try
		{
			if (await _tokenRepo.GetToken() != null)
				throw new TokenAlreadyRegisteredException();
		}
		catch (ResourceNotFoundException)
		{
		}

		_identityManager.ClearIdentitiesOfType<TokenIdentity>();

		return Ok(await _tokenRepo.RegisterToken(tokenDto.Name));
	}

	[HttpDelete]
	public async Task<IActionResult> DeleteToken()
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		if (identity is TokenIdentity)
			throw new TokenCannotManageException();

		await _tokenRepo.DeleteToken();

		_identityManager.ClearIdentitiesOfType<TokenIdentity>();

		return Ok();
	}

	[HttpGet]
	public async Task<IActionResult> GetTokenInfo()
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		if (identity is TokenIdentity)
			throw new TokenCannotManageException();

		var apiToken = await _tokenRepo.GetToken();

		apiToken.TokenHash = null;
		apiToken.TokenSalt = null;

		return Ok(apiToken);
	}
}