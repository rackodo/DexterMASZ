using MASZ.Bot.DTOs;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.Messaging.Enums;
using MASZ.Messaging.Translators;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Messaging.Controllers;

[ApiController]
[Route("api/v1/enums")]
public class ScheduledEnumController : ControllerBase
{
	private readonly Translation _translator;

	public ScheduledEnumController(Translation translator)
	{
		_translator = translator;
	}

	[HttpGet("scheduledmessagefailurereason")]
	public IActionResult FailureReason([FromQuery] Language? language = null)
	{
		_translator.SetLanguage(language);

		var enums = new List<EnumDto>();

		foreach (var enumValue in Enum.GetValues<ScheduledMessageFailureReason>())
			enums.Add(new EnumDto((int)enumValue, _translator.Get<MessagingEnumTranslator>().Enum(enumValue)));

		return Ok(enums);
	}

	[HttpGet("scheduledmessagestatus")]
	public IActionResult Status([FromQuery] Language? language = null)
	{
		_translator.SetLanguage(language);

		var enums = new List<EnumDto>();

		foreach (var enumValue in Enum.GetValues<ScheduledMessageStatus>())
			enums.Add(new EnumDto((int)enumValue, _translator.Get<MessagingEnumTranslator>().Enum(enumValue)));

		return Ok(enums);
	}
}
