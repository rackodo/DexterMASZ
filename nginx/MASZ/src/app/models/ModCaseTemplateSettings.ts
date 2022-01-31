export interface ModCaseTemplateSettings {
    name: string;
    viewPermission: ModCaseTemplateExpandedPermission
}

export enum ModCaseTemplateExpandedPermission {
    Global,
    DiscordGuild,
    Self
}

export function ModCaseTemplateExpandedPermissionOptions() : Array<string> {
    let keys = Object.keys(ModCaseTemplateExpandedPermission);
    return keys.slice(keys.length / 2);
}
