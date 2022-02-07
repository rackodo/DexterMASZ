using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bot.DTOs;

public class UploadedFileDto
{
	[Required] public IFormFile File { set; get; }
}