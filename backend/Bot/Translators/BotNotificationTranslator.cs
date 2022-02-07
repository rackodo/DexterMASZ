using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Translators;

public class BotNotificationTranslator : Translator
{
	public string NotificationRegisterWelcomeToMasz()
	{
		return PreferredLanguage switch
		{
			Language.De => "Willkommen bei Dexter!",
			Language.At => "Servus bei Dexter!",
			Language.Fr => "Bienvenue à Dexter !",
			Language.Es => "¡Bienvenido a Dexter!",
			Language.Ru => "Добро пожаловать в МАСЗ!",
			Language.It => "Benvenuto in Dexter!",
			_ => "Welcome to Dexter!"
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
				"Vielen Dank für deine Registrierung.\nIm Folgenden wirst du einige nützliche Tipps zum Einrichten und Verwenden von **Dexter** erhalten.",
			Language.At =>
				"Donksche fia dei Registrierung.\nDu siachst glei ei poar nützliche Tipps zum Eirichtn und Vawendn vo **Dexter**.",
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
	}

	public string NotificationRegisterUseFeaturesCommand()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Benutze den `/features` Befehl um zu sehen welche Features von **Dexter** dein aktuelles Setup unterstützt.",
			Language.At =>
				"Nutz den `/features` Beföhl um nochzumschauen wöchane Features dei aktuelles **Dexter**  Setup untastützn tuad.",
			Language.Fr =>
				"Utilisez la commande `/features` pour tester si votre configuration de guilde actuelle prend en charge toutes les fonctionnalités de **Dexter**.",
			Language.Es =>
				"Usa el comando `/ features` para probar si la configuración de tu gremio actual es compatible con todas las características de **Dexter**.",
			Language.Ru =>
				"Используйте команду `/ features`, чтобы проверить, поддерживает ли ваша текущая настройка гильдии все функции **Dexter**.",
			Language.It =>
				"Usa il comando `/features` per verificare se l'attuale configurazione della gilda supporta tutte le funzionalità di **Dexter**.",
			_ => "Use the `/features` command to test if your current guild setup supports all features of **Dexter**."
		};
	}

	public string NotificationRegisterDefaultLanguageUsed(string language)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Dexter wird `{language}` als Standard-Sprache für diese Gilde verwenden, wenn möglich.",
			Language.At => $"Dei Dexter wiad `{language}` ois Standard-Sproch fia die Güde nehma, wenns geht.",
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
	}

	public string NotificationRegisterConfusingTimestamps()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Zeitzonen können kompliziert sein.\nDexter benutzt ein Discord-Feature um Zeitstempel in der lokalen Zeitzone deines Computers/Handys anzuzeigen.",
			Language.At =>
				"De Zeitzonen kennan a weng schwer san.\nDexter nutzt a Discord-Feature um Zeitstempl in da lokalen Zeitzon vo deim PC/Handy ozumzeign.",
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
	}

	public string NotificationRegisterSupport()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Bitte wende dich an den [Dexter Support Server](https://discord.gg/5zjpzw6h3S) für weitere Fragen.",
			Language.At =>
				"Bitte wend di on den [Dexter Support Server](https://discord.gg/5zjpzw6h3S) fia weitare Frogn.",
			Language.Fr =>
				"Veuillez vous référer au [serveur de support Dexter] (https://discord.gg/5zjpzw6h3S) pour d'autres questions.",
			Language.Es =>
				"Consulte el [servidor de soporte Dexter] (https://discord.gg/5zjpzw6h3S) si tiene más preguntas.",
			Language.Ru =>
				"Дополнительные вопросы можно найти на [сервере поддержки Dexter] (https://discord.gg/5zjpzw6h3S).",
			Language.It =>
				"Fare riferimento al [Server di supporto Dexter] (https://discord.gg/5zjpzw6h3S) per ulteriori domande.",
			_ => "Please refer to the [Dexter Support Server](https://discord.gg/5zjpzw6h3S) for further questions."
		};
	}
}