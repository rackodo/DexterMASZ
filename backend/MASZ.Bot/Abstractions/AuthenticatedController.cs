using MASZ.Bot.Services;
using Microsoft.AspNetCore.Authorization;

namespace MASZ.Bot.Abstractions;

[Authorize]
public abstract class AuthenticatedController : BaseController
{
	private readonly IdentityManager _identityManager;
	private readonly Repository[] _repositories;

	protected AuthenticatedController(IdentityManager identityManager, params Repository[] repositories)
	{
		_identityManager = identityManager;
		_repositories = repositories;
	}

	protected async Task<Identity> SetupAuthentication()
	{
		var _identity = await _identityManager.GetIdentity(HttpContext);

		foreach (var repo in _repositories)
			repo.AsUser(_identity);

		return _identity;
	}
}