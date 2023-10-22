namespace Bot.DTOs;

public class EnumDto(int key, string value)
{
    public int Key { get; set; } = key;
    public string Value { get; set; } = value;
}
