using Bot.Abstractions;
using Bot.DTOs;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Punishments.Enums;
using Punishments.Translators;

namespace Punishments.Controllers;

[Route("api/v1/enums/")]
public class PunishmentEnumController : EnumController<PunishmentEnumTranslator>
{
    public PunishmentEnumController(Translation translator, ILogger<EnumController<PunishmentEnumTranslator>> logger) :
        base(translator, logger)
    {
    }

    [HttpGet("punishment")]
    public IActionResult Punishment([FromQuery] Language? language = null) =>
        TranslateEnum<PunishmentType>();

    [HttpGet("severity")]
    public IActionResult Severity([FromQuery] Language? language = null) =>
        TranslateEnum<SeverityType>();

    [HttpGet("casecreationtype")]
    public IActionResult CreationType([FromQuery] Language? language = null) =>
        TranslateEnum<CaseCreationType>();

    [HttpGet("lockedcommentstatus")]
    public IActionResult ViewLockedCommentStatus([FromQuery] Language? language = null) =>
        TranslateEnum<LockedCommentStatus>();

    [HttpGet("markedtodeletestatus")]
    public IActionResult ViewMarkedToDeleteStatus([FromQuery] Language? language = null) =>
        TranslateEnum<MarkedToDeleteStatus>();

    [HttpGet("punishmentactivestatus")]
    public IActionResult ViewPunishmentActiveStatus([FromQuery] Language? language = null) =>
        TranslateEnum<PunishmentActiveStatus>();
}
