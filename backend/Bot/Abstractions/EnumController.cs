using Bot.DTOs;
using Bot.Enums;
using Bot.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bot.Abstractions;

public class EnumController<TTranslator> : BaseController
{
    private readonly Translation _translator;
    private readonly ILogger<EnumController<TTranslator>> _logger;

    public EnumController(Translation translator, ILogger<EnumController<TTranslator>> logger)
    {
        _translator = translator;
        _logger = logger;
    }

    public ObjectResult ReturnEnumValue<TEnumType>([FromQuery] Language? language = null)
        where TEnumType : struct, Enum
    {
        _translator.SetLanguage(language);

        var translator = _translator.Get<TTranslator>();

        var method = typeof(TTranslator).GetMethods()
            .FirstOrDefault(s =>
                {
                    var parameters = s.GetParameters();

                    return
                        parameters.Length == 1 &&
                        parameters.Any(a => a.ParameterType == typeof(TEnumType)) &&
                        s.ReturnParameter?.ParameterType == typeof(string);
                }
            );

        if (method == null)
        {
            _logger.LogError("Could not find a method to translate the enum of {Enum} for {Translator}",
                nameof(TEnumType), nameof(TTranslator));
            return Problem("Translator does not exist for this enum.");
        }

        var enums = Enum.GetValues<TEnumType>().Select(enumValue =>
            new EnumDto(
                enumValue.GetHashCode(),
                ((string)method.Invoke(translator, new object[] { enumValue })).Humanize()
            )
        ).ToList();

        return Ok(enums);
    }

}
