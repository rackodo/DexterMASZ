using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MASZ.Bot.DTOs;

public class UploadedFileDto
{
	[Required] public IFormFile File { set; get; }
}