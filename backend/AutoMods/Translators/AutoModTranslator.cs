using Bot.Abstractions;
using Bot.Enums;

namespace AutoMods.Translators;

public class AutoModTranslator : Translator
{
    public string AutoMod() =>
        PreferredLanguage switch
        {
            Language.De => "Auto Mod",
            Language.Fr => "Mode Automatique",
            Language.Es => "Modo Automático",
            Language.Ru => "Авто Мод",
            Language.It => "Mod Automatico",
            _ => "Auto Mod"
        };
}