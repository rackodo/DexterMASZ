using Bot.Abstractions;
using Bot.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Controllers;

[Route("api/v1")]
public class AuthenticationController : BaseController
{
	private readonly IdentityManager _identityManager;

	public AuthenticationController(IdentityManager identityManager)
	{
		_identityManager = identityManager;
	}

	[HttpGet("login")]
	public IActionResult Login([FromQuery] string returnUrl)
	{
		if (string.IsNullOrEmpty(returnUrl))
			returnUrl = "/";

		var properties = new AuthenticationProperties
		{
			RedirectUri = returnUrl,
			Items =
			{
				{ "LoginProvider", "Discord" },
				{ "scheme", "Discord" }
			},
			AllowRefresh = true
		};

		return Challenge(properties, "Discord");
	}

	[HttpGet("logout")]
	[HttpPost("logout")]
	[Authorize]
	public IActionResult Logout()
	{
		_identityManager.RemoveIdentity(HttpContext);

		var properties = new AuthenticationProperties
		{
			RedirectUri = "/",
			Items =
			{
				{ "LoginProvider", "Discord" },
				{ "scheme", "Discord" }
			},
			AllowRefresh = true
		};

		return SignOut(properties, CookieAuthenticationDefaults.AuthenticationScheme);
	}
}