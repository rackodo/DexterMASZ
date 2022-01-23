using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.Punishments.Events;
using MASZ.Punishments.Exceptions;
using MASZ.Punishments.Models;

namespace MASZ.Punishments.Data;

public class CaseTemplateRepository : Repository
{
	private const int MaxAllowedCaseTemplatesPerUser = 20;
	private readonly PunishmentEventHandler _eventHandler;

	private readonly PunishmentDatabase _punishmentDatabase;

	public CaseTemplateRepository(PunishmentDatabase punishmentDatabase, PunishmentEventHandler eventHandler,
		DiscordRest discordRest) : base(discordRest)
	{
		_punishmentDatabase = punishmentDatabase;
		_eventHandler = eventHandler;
	}
	
	public async Task<CaseTemplate> CreateTemplate(CaseTemplate template)
	{
		var existingTemplates = await _punishmentDatabase.GetAllTemplatesFromUser(template.UserId);

		if (existingTemplates.Count >= MaxAllowedCaseTemplatesPerUser)
			throw new TooManyTemplatesCreatedException();

		template.CreatedAt = DateTime.UtcNow;
		template.UserId = Identity.Id;

		await _punishmentDatabase.SaveCaseTemplate(template);

		_eventHandler.CaseTemplateCreatedEvent.Invoke(template);

		return template;
	}

	public async Task<CaseTemplate> GetTemplate(int id)
	{
		var template = await _punishmentDatabase.GetSpecificCaseTemplate(id);

		if (template == null)
			throw new ResourceNotFoundException();

		return template;
	}

	public async Task DeleteTemplate(CaseTemplate template)
	{
		await _punishmentDatabase.DeleteSpecificCaseTemplate(template);

		_eventHandler.CaseTemplateDeletedEvent.Invoke(template);
	}

	public async Task<List<CaseTemplate>> GetTemplatesBasedOnPermissions(Identity identity)
	{
		var templates = await _punishmentDatabase.GetAllCaseTemplates();
		List<CaseTemplate> filteredTemplates = new();

		foreach (var template in templates)
			if (await AllowedToView(template, identity))
				filteredTemplates.Add(template);

		return filteredTemplates;
	}

	private async Task<bool> AllowedToView(CaseTemplate template, Identity identity)
	{
		if (identity.GetCurrentUser().IsBot)
			return true;

		if (await identity.IsSiteAdmin())
			return true;

		if (template.UserId == Identity.Id)
			return true;

		return template.ViewPermission switch
		{
			ViewPermission.Self => false,
			ViewPermission.Global => true,
			_ => await identity.HasPermission(DiscordPermission.Moderator, template.CreatedForGuildId)
		};
	}
}