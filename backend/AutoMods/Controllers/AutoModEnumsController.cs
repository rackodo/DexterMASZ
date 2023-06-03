using AutoMods.Enums;
using AutoMods.Translators;
using Bot.Abstractions;
using Bot.DTOs;
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
    public IActionResult AutoModActions([FromQuery] Language? language = null) =>
        TranslateEnum<AutoModAction>();

    [HttpGet("automodchannelbehavior")]
    public IActionResult ChannelNotificationBehavior([FromQuery] Language? language = null) =>
        TranslateEnum<AutoModChannelNotificationBehavior>();

    [HttpGet("automodtype")]
    public IActionResult AutoModTypes([FromQuery] Language? language = null) =>
        TranslateEnum<AutoModType>();
}
