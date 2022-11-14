using System.ComponentModel.DataAnnotations;

namespace Punishments.DTOs;

public class ModCaseCommentForPutDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "A message is required.")]
    [MaxLength(300)]
    public string Message { get; set; }
}