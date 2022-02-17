import { ModCaseTemplate } from "./ModCaseTemplate";
import { DiscordUser } from "./DiscordUser";
import { DiscordGuild } from "./DiscordGuild";

export interface ModCaseTemplateExpanded {
    caseTemplate: ModCaseTemplate;
    creator?: DiscordUser;
    guild: DiscordGuild;
}