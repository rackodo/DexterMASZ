export interface AppSettings {
	clientId: string;
	discordBotToken: string;
	clientSecret: string;
	absolutePathToFileUpload: string;
	serviceHostName: string;
	serviceDomain: string;
	serviceBaseUrl: string;
	siteAdmins: string[];
    embedTitle: string;
    embedContent: string;
    defaultLanguage: number;
    auditLogWebhookUrl: string;
    publicFileMode: boolean;
	demoModeEnabled: boolean;
	corsEnabled: boolean;
}