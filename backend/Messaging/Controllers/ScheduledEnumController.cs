using Bot.Abstractions;
using Bot.DTOs;
using Bot.Enums;
using Bot.Services;
using Messaging.Enums;
using Messaging.Translators;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Controllers;

[Route("api/v1/enums")]
public class ScheduledEnumController : BaseController
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
