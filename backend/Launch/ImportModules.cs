using MASZ.AutoMods;
using MASZ.Bot;
using MASZ.Bot.Abstractions;
using MASZ.GuildAudits;
using MASZ.Invites;
using MASZ.Messaging;
using MASZ.MOTDs;
using MASZ.Punishments;
using MASZ.UserMaps;
using MASZ.UserNotes;
using MASZ.Utilities;

namespace Launch;

public static class ImportModules
{
	public static List<Module> GetModules()
	{
		return new List<Module>
		{
			new BotModule(),
			new BotWebModule(),
			new AutoModModule(),
			new GuildAuditModule(),
			new InviteModule(),
			new MotdModule(),
			new PunishmentModule(),
			new UserMapModule(),
			new UserNoteModule(),
			new UtilityModule(),
			new MessagingModule()
		};
	}
}