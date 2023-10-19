namespace Punishments.Models;

public class ModCaseTable(List<ModCaseTableEntry> modCase, int fullSize)
{
    public List<ModCaseTableEntry> Cases { get; set; } = modCase;
    public int FullSize { get; set; } = fullSize;
}
