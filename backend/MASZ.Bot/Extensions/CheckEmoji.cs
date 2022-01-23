namespace MASZ.Bot.Extensions;

public static class CheckEmoji
{
	private const char CHECK = '✅';
	private const char X_CHECK = '❌';

	public static char GetCheckEmoji(this bool value)
	{
		return value ? CHECK : X_CHECK;
	}
}
