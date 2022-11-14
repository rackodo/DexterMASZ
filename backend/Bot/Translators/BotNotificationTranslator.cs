using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Translators;

public class BotNotificationTranslator : Translator
{
    public string NotificationRegisterWelcomeToDexter() =>
        PreferredLanguage switch
        {
            Language.De => "Willkommen bei Dexter!",
            Language.Fr => "Bienvenue à Dexter !",
            Language.Es => "¡Bienvenido a Dexter!",
            Language.Ru => "Добро пожаловать в Dexter!",
            Language.It => "Benvenuto in Dexter!",
            _ => "Welcome to Dexter!"
        };

    public string NotificationFilesCreate() =>
        PreferredLanguage switch
        {
            Language.De => "Datei hochgeladen",
            Language.Fr => "Fichier téléchargé",
            Language.Es => "Archivo subido",
            Language.Ru => "Файл загружен",
            Language.It => "File caricato",
            _ => "File uploaded"
        };

    public string NotificationFilesDelete() =>
        PreferredLanguage switch
        {
            Language.De => "Datei gelöscht",
            Language.Fr => "Fichier supprimé",
            Language.Es => "Archivo eliminado",
            Language.Ru => "Файл удален",
            Language.It => "File cancellato",
            _ => "File deleted"
        };

    public string NotificationFilesUpdate() =>
        PreferredLanguage switch
        {
            Language.De => "Datei aktualisiert",
            Language.Fr => "Fichier mis à jour",
            Language.Es => "Archivo actualizado",
            Language.Ru => "Файл обновлен",
            Language.It => "File aggiornato",
            _ => "File updated"
        };

    public string NotificationRegisterDescriptionThanks() =>
        PreferredLanguage switch
        {
            Language.De =>
                "Vielen Dank für deine Registrierung.\nIm Folgenden wirst du einige nützliche Tipps zum Einrichten und Verwenden von **Dexter** erhalten.",
            Language.Fr =>
                "Merci d'avoir enregistré votre guilde.\nDans ce qui suit, vous apprendrez quelques conseils utiles pour configurer et utiliser **Dexter**.",
            Language.Es =>
                "Gracias por registrar tu gremio.\nA continuación, aprenderá algunos consejos útiles para configurar y usar **Dexter**.",
            Language.Ru =>
                "Спасибо за регистрацию вашей гильдии.\nДалее вы получите несколько полезных советов по настройке и использованию **Dexter**.",
            Language.It =>
                "Grazie per aver registrato la tua gilda.\nDi seguito imparerai alcuni suggerimenti utili per impostare e utilizzare **Dexter**.",
            _ =>
                "Thanks for registering your guild.\nIn the following you will learn some useful tips for setting up and using **Dexter**."
        };

    public string NotificationRegisterDefaultLanguageUsed(string language) =>
        PreferredLanguage switch
        {
            Language.De => $"Dexter wird `{language}` als Standard-Sprache für diese Gilde verwenden, wenn möglich.",
            Language.Fr =>
                $"Dexter utilisera `{language}` comme langue par défaut pour cette guilde dans la mesure du possible.",
            Language.Es =>
                $"Dexter usará `{language}` como idioma predeterminado para este gremio siempre que sea posible.",
            Language.Ru =>
                $"Dexter будет использовать `{language}` как язык по умолчанию для этой гильдии, когда это возможно.",
            Language.It =>
                $"Dexter utilizzerà `{language}` come lingua predefinita per questa gilda ogni volta che sarà possibile.",
            _ => $"Dexter will use `{language}` as default language for this guild whenever possible."
        };

    public string NotificationRegisterConfusingTimestamps() =>
        PreferredLanguage switch
        {
            Language.De =>
                "Zeitzonen können kompliziert sein.\nDexter benutzt ein Discord-Feature um Zeitstempel in der lokalen Zeitzone deines Computers/Handys anzuzeigen.",
            Language.Fr =>
                "Les fuseaux horaires peuvent être déroutants.\nDexter utilise une fonction Discord pour afficher les horodatages dans le fuseau horaire local de votre ordinateur/téléphone.",
            Language.Es =>
                "Las zonas horarias pueden resultar confusas.\nDexter usa una función de Discord para mostrar marcas de tiempo en la zona horaria local de su computadora / teléfono.",
            Language.Ru =>
                "Часовые пояса могут сбивать с толку.\nDexter использует функцию Discord для отображения меток времени в местном часовом поясе вашего компьютера / телефона.",
            Language.It =>
                "I fusi orari possono creare confusione.\nDexter utilizza una funzione Discord per visualizzare i timestamp nel fuso orario locale del tuo computer/telefono.",
            _ =>
                "Timezones can be confusing.\nDexter uses a Discord feature to display timestamps in the local timezone of your computer/phone."
        };

    public string NotificationRegisterSupport() =>
        PreferredLanguage switch
        {
            Language.De =>
                "Bitte wende dich an den [Dexter Support Server](https://discord.gg/DBS664yjWN) für weitere Fragen.",
            Language.Fr =>
                "Veuillez vous référer au [serveur de support Dexter] (https://discord.gg/DBS664yjWN) pour d'autres questions.",
            Language.Es =>
                "Consulte el [servidor de soporte Dexter] (https://discord.gg/DBS664yjWN) si tiene más preguntas.",
            Language.Ru =>
                "Дополнительные вопросы можно найти на [сервере поддержки Dexter] (https://discord.gg/DBS664yjWN).",
            Language.It =>
                "Fare riferimento al [Server di supporto Dexter] (https://discord.gg/DBS664yjWN) per ulteriori domande.",
            _ => "Please refer to the [Dexter Support Server](https://discord.gg/DBS664yjWN) for further questions."
        };
}