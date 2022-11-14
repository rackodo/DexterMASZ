using Bot.Abstractions;
using Bot.DTOs;
using Bot.Enums;
using Bot.Services;
using Bot.Translators;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Controllers;

[Route("api/v1/enums")]
public class BotEnumControllers : BaseController
{
    private readonly Translation _translator;

    public BotEnumControllers(Translation translator) => _translator = translator;

    [HttpGet("apierror")]
    public IActionResult CreationType([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<ApiError>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<BotEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }

    [HttpGet("editstatus")]
    public IActionResult ViewEditStatus([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<EditStatus>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<BotEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }

    [HttpGet("language")]
    public IActionResult LanguageEnum([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<Language>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<BotEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }

    [HttpGet("viewpermission")]
    public IActionResult ViewPermissions([FromQuery] Language? language = null)
    {
        _translator.SetLanguage(language);

        var enums = new List<EnumDto>();

        foreach (var enumValue in Enum.GetValues<ViewPermission>())
            enums.Add(new EnumDto((int)enumValue, _translator.Get<BotEnumTranslator>().Enum(enumValue)));

        return Ok(enums);
    }
}