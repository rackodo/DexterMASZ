namespace Bot.DTOs;

public class EnumDto
{
	public EnumDto(int key, string value)
	{
		Key = key;
		Value = value;
	}

	public int Key { get; set; }
	public string Value { get; set; }
}