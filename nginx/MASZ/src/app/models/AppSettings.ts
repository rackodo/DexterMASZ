export interface AppSettings {
	clientId: bigint;
	discordBotToken: string;
	clientSecret: string;
	absolutePathToFileUpload: string;
	serviceHostName: string;
	serviceDomain: string;
	serviceBaseUrl: string;
	siteAdmins: bigint[];
    embedTitle: string;
    embedContent: string;
    defaultLanguage: number;
    auditLogWebhookUrl: string;
    publicFileMode: boolean;
	demoModeEnabled: boolean;
	corsEnabled: boolean;
}