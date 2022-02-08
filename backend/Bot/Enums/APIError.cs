namespace Bot.Enums;

public enum ApiError
{
	Unknown,

	// INTERNAL
	InvalidDiscordUser,
	ResourceNotFound,
	InvalidIdentity,
	GuildUnregistered,
	Unauthorized,
	GuildAlreadyRegistered,
	NotAllowedInDemoMode,
	RoleNotFound,
	ResourceAlreadyExists,
	InvalidFilePath,
	NoGuildsRegistered,
	InvalidAuditLogEvent,
	NotFoundInCache,

	// USER MAPS
	CannotBeSameUser,

	// MODCASES
	ProtectedModCaseSuspect,
	ProtectedModCaseSuspectIsBot,
	ProtectedModCaseSuspectIsSiteAdmin,
	ProtectedModCaseSuspectIsTeam,
	ModCaseIsMarkedToBeDeleted,
	ModCaseIsNotMarkedToBeDeleted,
	ModCaseDoesNotAllowComments,
	TooManyTemplates,
	LastCommentAlreadyFromSuspect,

	// AUTOMOD
	InvalidAutoModAction,
	InvalidAutoModType,

	// SCHEDULED MESSAGES
	ProtectedScheduledMessage,
	InvalidDateForScheduledMessage
}