namespace Bot.Extensions;

public static class CropTextToLength
{
    public static string CropText(this string text, int stringLength) =>
        text.Length <= 100 ? text : string.Join("", text.Take(stringLength > 3 ? stringLength - 3 : 0)) + "...";
}
