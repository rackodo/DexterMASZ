export interface AppSettings {
	clientId: string;
	discordBotToken: string;
	clientSecret: string;
	absolutePathToFileUpload: string;
	serviceDomain: string;
	serviceBaseUrl: string;
	siteAdmins: string[];
    embedTitle: string;
    embedContent: string;
    defaultLanguage: number;
    auditLogWebhookUrl: string;
	corsEnabled: boolean;
}
