using System.ComponentModel.DataAnnotations;

namespace Bot.DTOs;

public class TokenForCreateDto
{
	[Required(ErrorMessage = "Name field is required", AllowEmptyStrings = false)]
	public string Name { get; set; }
}