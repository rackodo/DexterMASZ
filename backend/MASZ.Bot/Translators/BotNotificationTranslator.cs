using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Translators;

public class BotNotificationTranslator : Translator
{
	public string NotificationRegisterWelcomeToMasz()
	{
		return PreferredLanguage switch
		{
			Language.De => "Willkommen bei MASZ!",
			Language.At => "Servus bei MASZ!",
			Language.Fr => "Bienvenue à MASZ !",
			Language.Es => "¡Bienvenido a MASZ!",
			Language.Ru => "Добро пожаловать в МАСЗ!",
			Language.It => "Benvenuto in MASZ!",
			_ => "Welcome to MASZ!"
		};
	}

	public string NotificationFilesCreate()
	{
		return PreferredLanguage switch
		{
			Language.De => "Datei hochgeladen",
			Language.At => "Datei hochglodn",
			Language.Fr => "Fichier téléchargé",
			Language.Es => "Archivo subido",
			Language.Ru => "Файл загружен",
			Language.It => "File caricato",
			_ => "File uploaded"
		};
	}

	public string NotificationFilesDelete()
	{
		return PreferredLanguage switch
		{
			Language.De => "Datei gelöscht",
			Language.At => "Datei glescht",
			Language.Fr => "Fichier supprimé",
			Language.Es => "Archivo eliminado",
			Language.Ru => "Файл удален",
			Language.It => "File cancellato",
			_ => "File deleted"
		};
	}

	public string NotificationFilesUpdate()
	{
		return PreferredLanguage switch
		{
			Language.De => "Datei aktualisiert",
			Language.At => "Datei aktualisiert",
			Language.Fr => "Fichier mis à jour",
			Language.Es => "Archivo actualizado",
			Language.Ru => "Файл обновлен",
			Language.It => "File aggiornato",
			_ => "File updated"
		};
	}

	public string NotificationRegisterDescriptionThanks()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Vielen Dank für deine Registrierung.\nIm Folgenden wirst du einige nützliche Tipps zum Einrichten und Verwenden von **MASZ** erhalten.",
			Language.At =>
				"Donksche fia dei Registrierung.\nDu siachst glei ei poar nützliche Tipps zum Eirichtn und Vawendn vo **MASZ**.",
			Language.Fr =>
				"Merci d'avoir enregistré votre guilde.\nDans ce qui suit, vous apprendrez quelques conseils utiles pour configurer et utiliser **MASZ**.",
			Language.Es =>
				"Gracias por registrar tu gremio.\nA continuación, aprenderá algunos consejos útiles para configurar y usar **MASZ**.",
			Language.Ru =>
				"Спасибо за регистрацию вашей гильдии.\nДалее вы получите несколько полезных советов по настройке и использованию **MASZ**.",
			Language.It =>
				"Grazie per aver registrato la tua gilda.\nDi seguito imparerai alcuni suggerimenti utili per impostare e utilizzare **MASZ**.",
			_ =>
				"Thanks for registering your guild.\nIn the following you will learn some useful tips for setting up and using **MASZ**."
		};
	}

	public string NotificationRegisterUseFeaturesCommand()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Benutze den `/features` Befehl um zu sehen welche Features von **MASZ** dein aktuelles Setup unterstützt.",
			Language.At =>
				"Nutz den `/features` Beföhl um nochzumschauen wöchane Features dei aktuelles **MASZ**  Setup untastützn tuad.",
			Language.Fr =>
				"Utilisez la commande `/features` pour tester si votre configuration de guilde actuelle prend en charge toutes les fonctionnalités de **MASZ**.",
			Language.Es =>
				"Usa el comando `/ features` para probar si la configuración de tu gremio actual es compatible con todas las características de **MASZ**.",
			Language.Ru =>
				"Используйте команду `/ features`, чтобы проверить, поддерживает ли ваша текущая настройка гильдии все функции **MASZ**.",
			Language.It =>
				"Usa il comando `/features` per verificare se l'attuale configurazione della gilda supporta tutte le funzionalità di **MASZ**.",
			_ => "Use the `/features` command to test if your current guild setup supports all features of **MASZ**."
		};
	}

	public string NotificationRegisterDefaultLanguageUsed(string language)
	{
		return PreferredLanguage switch
		{
			Language.De => $"MASZ wird `{language}` als Standard-Sprache für diese Gilde verwenden, wenn möglich.",
			Language.At => $"Dei MASZ wiad `{language}` ois Standard-Sproch fia die Güde nehma, wenns geht.",
			Language.Fr =>
				$"MASZ utilisera `{language}` comme langue par défaut pour cette guilde dans la mesure du possible.",
			Language.Es =>
				$"MASZ usará `{language}` como idioma predeterminado para este gremio siempre que sea posible.",
			Language.Ru =>
				$"MASZ будет использовать `{language}` как язык по умолчанию для этой гильдии, когда это возможно.",
			Language.It =>
				$"MASZ utilizzerà `{language}` come lingua predefinita per questa gilda ogni volta che sarà possibile.",
			_ => $"MASZ will use `{language}` as default language for this guild whenever possible."
		};
	}

	public string NotificationRegisterConfusingTimestamps()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Zeitzonen können kompliziert sein.\nMASZ benutzt ein Discord-Feature um Zeitstempel in der lokalen Zeitzone deines Computers/Handys anzuzeigen.",
			Language.At =>
				"De Zeitzonen kennan a weng schwer san.\nMASZ nutzt a Discord-Feature um Zeitstempl in da lokalen Zeitzon vo deim PC/Handy ozumzeign.",
			Language.Fr =>
				"Les fuseaux horaires peuvent être déroutants.\nMASZ utilise une fonction Discord pour afficher les horodatages dans le fuseau horaire local de votre ordinateur/téléphone.",
			Language.Es =>
				"Las zonas horarias pueden resultar confusas.\nMASZ usa una función de Discord para mostrar marcas de tiempo en la zona horaria local de su computadora / teléfono.",
			Language.Ru =>
				"Часовые пояса могут сбивать с толку.\nMASZ использует функцию Discord для отображения меток времени в местном часовом поясе вашего компьютера / телефона.",
			Language.It =>
				"I fusi orari possono creare confusione.\nMASZ utilizza una funzione Discord per visualizzare i timestamp nel fuso orario locale del tuo computer/telefono.",
			_ =>
				"Timezones can be confusing.\nMASZ uses a Discord feature to display timestamps in the local timezone of your computer/phone."
		};
	}

	public string NotificationRegisterSupport()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Bitte wende dich an den [MASZ Support Server](https://discord.gg/5zjpzw6h3S) für weitere Fragen.",
			Language.At =>
				"Bitte wend di on den [MASZ Support Server](https://discord.gg/5zjpzw6h3S) fia weitare Frogn.",
			Language.Fr =>
				"Veuillez vous référer au [serveur de support MASZ] (https://discord.gg/5zjpzw6h3S) pour d'autres questions.",
			Language.Es =>
				"Consulte el [servidor de soporte MASZ] (https://discord.gg/5zjpzw6h3S) si tiene más preguntas.",
			Language.Ru =>
				"Дополнительные вопросы можно найти на [сервере поддержки MASZ] (https://discord.gg/5zjpzw6h3S).",
			Language.It =>
				"Fare riferimento al [Server di supporto MASZ] (https://discord.gg/5zjpzw6h3S) per ulteriori domande.",
			_ => "Please refer to the [MASZ Support Server](https://discord.gg/5zjpzw6h3S) for further questions."
		};
	}
}