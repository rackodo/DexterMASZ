namespace Bot.Extensions;

public static class CheckEmoji
{
    private const char CHECK = '✅';
    private const char X_CHECK = '❌';

    public static char GetCheckEmoji(this bool value) => value ? CHECK : X_CHECK;
}