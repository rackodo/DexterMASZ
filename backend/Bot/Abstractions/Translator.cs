using Bot.Enums;

namespace Bot.Abstractions;

public abstract class Translator
{
    public Language PreferredLanguage { get; set; }

    protected Translator(Language preferredLanguage = Language.En) => PreferredLanguage = preferredLanguage;
}
