using AutoMods.Enums;
using AutoMods.Translators;
using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoMods.Controllers;

[Route("api/v1/enums")]
public class AutoModEnumsController : EnumController<AutoModEnumTranslator>
{
    public AutoModEnumsController(Translation translator, ILogger<EnumController<AutoModEnumTranslator>> logger) :
        base(translator, logger)
    {
    }

    [HttpGet("automodaction")]
    public async Task<IActionResult> AutoModActions([FromQuery] Language? language = null) =>
        await TranslateEnum<AutoModAction>(language);

    [HttpGet("automodchannelbehavior")]
    public async Task<IActionResult> ChannelNotificationBehavior([FromQuery] Language? language = null) =>
        await TranslateEnum<AutoModChannelNotificationBehavior>(language);

    [HttpGet("automodtype")]
    public async Task<IActionResult> AutoModTypes([FromQuery] Language? language = null) =>
        await TranslateEnum<AutoModType>(language);
}
