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
    RoleNotFound,
    ResourceAlreadyExists,
    InvalidFilePath,
    NoGuildsRegistered,
    InvalidAuditLogEvent,
    NotFoundInCache,
    GuildNotFound,

    // USER MAPS
    CannotBeSameUser,

    // MOD CASES
    ProtectedModCaseSuspect,
    ProtectedModCaseSuspectIsBot,
    ProtectedModCaseSuspectIsSiteAdmin,
    ProtectedModCaseSuspectIsTeam,
    ModCaseIsMarkedToBeDeleted,
    ModCaseIsNotMarkedToBeDeleted,
    ModCaseDoesNotAllowComments,
    TooManyTemplates,
    LastCommentAlreadyFromSuspect,
    AlreadyFinalWarned,

    // AUTO MOD
    InvalidAutoModAction,
    InvalidAutoModType,

    // SCHEDULED MESSAGES
    ProtectedScheduledMessage,
    InvalidDateForScheduledMessage,

    UserNotInVoiceChannel
}
