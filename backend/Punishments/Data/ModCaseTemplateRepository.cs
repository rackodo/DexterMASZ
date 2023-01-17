using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;
using Punishments.Events;
using Punishments.Exceptions;
using Punishments.Models;

namespace Punishments.Data;

public class ModCaseTemplateRepository : Repository, IDeleteGuildData
{
    private const int MaxAllowedModCaseTemplatesPerUser = 20;
    private readonly PunishmentEventHandler _eventHandler;

    private readonly PunishmentDatabase _punishmentDatabase;

    public ModCaseTemplateRepository(PunishmentDatabase punishmentDatabase, PunishmentEventHandler eventHandler,
        DiscordRest discordRest) : base(discordRest)
    {
        _punishmentDatabase = punishmentDatabase;
        _eventHandler = eventHandler;
    }

    public async Task DeleteGuildData(ulong guildId) => await _punishmentDatabase.DeleteAllTemplatesForGuild(guildId);

    public async Task<ModCaseTemplate> CreateTemplate(ModCaseTemplate template)
    {
        var existingTemplates = await _punishmentDatabase.GetAllTemplatesFromUser(template.UserId);

        if (existingTemplates.Count >= MaxAllowedModCaseTemplatesPerUser)
            throw new TooManyTemplatesCreatedException();

        template.CreatedAt = DateTime.UtcNow;
        template.UserId = Identity.Id;

        await _punishmentDatabase.SaveCaseTemplate(template);

        _eventHandler.CaseTemplateCreatedEvent.Invoke(template);

        return template;
    }

    public async Task<ModCaseTemplate> GetTemplate(int id)
    {
        var template = await _punishmentDatabase.GetSpecificCaseTemplate(id);

        return template == null ? throw new ResourceNotFoundException() : template;
    }

    public async Task DeleteTemplate(ModCaseTemplate template)
    {
        await _punishmentDatabase.DeleteSpecificCaseTemplate(template);

        _eventHandler.CaseTemplateDeletedEvent.Invoke(template);
    }

    public async Task<List<ModCaseTemplate>> GetTemplatesBasedOnPermissions(Identity identity)
    {
        var templates = await _punishmentDatabase.GetAllModCaseTemplates();
        List<ModCaseTemplate> filteredTemplates = new();

        foreach (var template in templates)
        {
            if (await AllowedToView(template, identity))
                filteredTemplates.Add(template);
        }

        return filteredTemplates;
    }

    private async Task<bool> AllowedToView(ModCaseTemplate template, Identity identity)
    {
        if (identity.GetCurrentUser().IsBot)
            return true;

        if (await identity.IsSiteAdmin())
            return true;

        return template.UserId == Identity.Id
            ? true
            : template.ViewPermission switch
        {
            ViewPermission.Self => false,
            ViewPermission.Global => true,
            _ => await identity.HasPermission(DiscordPermission.Moderator, template.CreatedForGuildId)
        };
    }
}
