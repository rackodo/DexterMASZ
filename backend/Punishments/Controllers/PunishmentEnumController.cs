using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Punishments.Enums;
using Punishments.Translators;

namespace Punishments.Controllers;

[Route("api/v1/enums/")]
public class PunishmentEnumController(Translation translator, ILogger<EnumController<PunishmentEnumTranslator>> logger) : EnumController<PunishmentEnumTranslator>(translator, logger)
{
    [HttpGet("punishment")]
    public async Task<IActionResult> Punishment([FromQuery] Language? language = null) =>
        await TranslateEnum<PunishmentType>(language);

    [HttpGet("severity")]
    public async Task<IActionResult> Severity([FromQuery] Language? language = null) =>
        await TranslateEnum<SeverityType>(language);

    [HttpGet("casecreationtype")]
    public async Task<IActionResult> CreationType([FromQuery] Language? language = null) =>
        await TranslateEnum<CaseCreationType>(language);

    [HttpGet("lockedcommentstatus")]
    public async Task<IActionResult> ViewLockedCommentStatus([FromQuery] Language? language = null) =>
        await TranslateEnum<LockedCommentStatus>(language);

    [HttpGet("markedtodeletestatus")]
    public async Task<IActionResult> ViewMarkedToDeleteStatus([FromQuery] Language? language = null) =>
        await TranslateEnum<MarkedToDeleteStatus>(language);

    [HttpGet("punishmentactivestatus")]
    public async Task<IActionResult> ViewPunishmentActiveStatus([FromQuery] Language? language = null) =>
        await TranslateEnum<PunishmentActiveStatus>(language);
}
