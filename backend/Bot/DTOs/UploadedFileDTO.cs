using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bot.DTOs;

public class UploadedFileDto
{
    [Required] public IFormFile File { set; get; }
}