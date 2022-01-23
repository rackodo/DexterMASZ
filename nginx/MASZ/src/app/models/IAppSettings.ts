export interface IAppSettings {
	clientId: number;
	discordBotToken: string;
	clientSecret: string;
	absolutePathToFileUpload: string;
	serviceHostName: string;
	serviceDomain: string;
	serviceBaseUrl: string;
	siteAdmins: number[];
    embedTitle: string;
    embedContent: string;
    defaultLanguage: number;
    auditLogWebhookUrl: string;
    publicFileMode: boolean;
	demoModeEnabled: boolean;
	corsEnabled: boolean;
}