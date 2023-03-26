using Bot.Abstractions;
using Bot.Enums;

namespace Music.Exceptions;

public class UserNotInVoiceChannel : ApiException
{
    public UserNotInVoiceChannel() :
        base("Please join a voice channel before using this command.",
            ApiError.UserNotInVoiceChannel)
    {
    }
}
