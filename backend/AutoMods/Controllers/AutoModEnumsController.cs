using AutoMods.Enums;
using AutoMods.Translators;
using Bot.Abstractions;
using Bot.DTOs;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoMods.Controllers;

[Route("api/v1/enums")]
public class AutoModEnumsController : BaseController
{
    private readonly Translation _translator;

    public AutoModEnumsController(Translation translator) => _translator = translator;

    [HttpGet("automodaction")]
    public IActionResult AutoModActions([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<AutoModAction>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<AutoModEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }

    [HttpGet("automodchannelbehavior")]
    public IActionResult ChannelNotificationBehavior([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<AutoModChannelNotificationBehavior>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<AutoModEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }

    [HttpGet("automodtype")]
    public IActionResult AutoModTypes([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<AutoModType>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<AutoModEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }
}
