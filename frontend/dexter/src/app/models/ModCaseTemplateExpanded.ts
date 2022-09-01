import { DiscordGuild } from "./DiscordGuild";
import { DiscordUser } from "./DiscordUser";
import { ModCaseTemplate } from "./ModCaseTemplate";

export interface ModCaseTemplateExpanded {
    caseTemplate: ModCaseTemplate;
    creator?: DiscordUser;
    guild: DiscordGuild;
}
