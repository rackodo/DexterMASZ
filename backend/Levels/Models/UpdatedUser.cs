namespace Levels.Models;

public class UpdatedUser
{
    public string Error { get; set; }
    public string AddedRoles { get; set; }
    public string RemovedRoles { get; set; }

    public bool IsErrored => !string.IsNullOrWhiteSpace(Error);
}
