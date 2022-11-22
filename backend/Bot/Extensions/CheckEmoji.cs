namespace Bot.Extensions;

public static class CheckEmoji
{
    private const char Check = '✅';
    private const char XCheck = '❌';

    public static char GetCheckEmoji(this bool value) => value ? Check : XCheck;
}
