using System.ComponentModel.DataAnnotations;

namespace MOTDs.DTOs;

public class MotdForCreateDto
{
    [Required(ErrorMessage = "Message field is required", AllowEmptyStrings = false)]
    public string Message { get; set; }

    public bool ShowMotd { get; set; } = true;
}