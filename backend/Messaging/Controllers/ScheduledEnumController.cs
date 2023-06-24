using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Messaging.Enums;
using Messaging.Translators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Messaging.Controllers;

[Route("api/v1/enums")]
public class ScheduledEnumController : EnumController<MessagingEnumTranslator>
{
    public ScheduledEnumController(Translation translator, ILogger<EnumController<MessagingEnumTranslator>> logger) :
        base(translator, logger)
    {
    }

    [HttpGet("scheduledmessagefailurereason")]
    public async Task<IActionResult> FailureReason([FromQuery] Language? language = null) =>
        await TranslateEnum<ScheduledMessageFailureReason>(language);

    [HttpGet("scheduledmessagestatus")]
    public async Task<IActionResult> Status([FromQuery] Language? language = null) =>
        await TranslateEnum<ScheduledMessageStatus>(language);
}
