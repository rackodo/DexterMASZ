export interface GuildLevelConfig {
    id: string;
    coefficients: number[];
    xpInterval: number;
    minimumTextXpGiven: number;
    maximumTextXpGiven: number;
    minimumVoiceXpGiven: number;
    maximumVoiceXpGiven: number;
    levelUpTemplate: string;
    voiceLevelUpChannel: string;
    textLevelUpChannel: string;

    disabledXpChannels: string[];
    handleRoles: boolean;
    sendTextLevelUps: boolean;
    sendVoiceLevelUps: boolean;
    voiceXpCountMutedMembers: boolean;
    voiceXpRequiredMembers: number;

    nicknameDisabledRole: string;
    nicknameDisabledReplacement: string;

    levels: {[id: number] : string[]}
    levelUpMessageOverrides: {[id: number] : string}
}
