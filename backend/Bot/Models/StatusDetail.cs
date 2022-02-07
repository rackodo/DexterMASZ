namespace Bot.Models;

public class StatusDetail
{
	public StatusDetail()
	{
		Online = true;
		LastDisconnect = null;
		ResponseTime = null;
		Message = null;
	}

	public bool Online { get; set; }
	public DateTime? LastDisconnect { get; set; }
	public double? ResponseTime { get; set; }
	public string Message { get; set; }
}