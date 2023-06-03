using Bot.Abstractions;
using Bot.DTOs;
using Bot.Enums;
using Bot.Services;
using Bot.Translators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bot.Controllers;

[Route("api/v1/enums")]
public class BotEnumControllers : EnumController<BotEnumTranslator>
{
    public BotEnumControllers(Translation translator, ILogger<EnumController<BotEnumTranslator>> logger) :
        base(translator, logger)
    {
    }

    [HttpGet("apierror")]
    public IActionResult ApiError([FromQuery] Language? language = null) =>
        TranslateEnum<ApiError>(language);

    [HttpGet("editstatus")]
    public IActionResult ViewEditStatus([FromQuery] Language? language = null) => 
        TranslateEnum<EditStatus>(language);

    [HttpGet("language")]
    public IActionResult LanguageEnum([FromQuery] Language? language = null) =>
        TranslateEnum<Language>(language);

    [HttpGet("viewpermission")]
    public IActionResult ViewPermissions([FromQuery] Language? language = null) =>
        TranslateEnum<ViewPermission>(language);
}
