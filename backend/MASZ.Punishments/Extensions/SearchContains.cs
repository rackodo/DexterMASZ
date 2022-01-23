using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.Punishments.Models;
using MASZ.Punishments.Translators;
using MASZ.Punishments.Views;

namespace MASZ.Punishments.Extensions;

public static class SearchContains
{
	public static bool Search(this string search, ModCaseTableEntry obj, Translation translator)
	{
		if (obj == null)
			return false;

		return search.Search(obj.ModCase, translator) ||
		       search.Search(obj.Moderator) ||
		       search.Search(obj.Suspect);
	}
	
	public static bool Search(this string search, CaseView obj, Translation translator)
	{
		if (obj == null)
			return false;

		return search.Search(obj.Title) ||
		       search.Search(obj.Description) ||
		       search.Search(translator.Get<PunishmentEnumTranslator>().Enum(obj.PunishmentType)) ||
		       search.Search(obj.Username) ||
		       search.Search(obj.Discriminator) ||
		       search.Search(obj.Nickname) ||
		       search.Search(obj.UserId) ||
		       search.Search(obj.ModId) ||
		       search.Search(obj.LastEditedByModId) ||
		       search.Search(obj.CreatedAt) ||
		       search.Search(obj.OccurredAt) ||
		       search.Search(obj.LastEditedAt) ||
		       search.Search(obj.Labels) ||
		       search.Search(obj.CaseId.ToString()) ||
		       search.Search($"#{obj.CaseId}");
	}
}