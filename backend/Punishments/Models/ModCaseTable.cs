namespace Punishments.Models;

public class ModCaseTable
{
	public ModCaseTable(List<ModCaseTableEntry> modCase, int fullSize)
	{
		Cases = modCase;
		FullSize = fullSize;
	}

	public List<ModCaseTableEntry> Cases { get; set; }
	public int FullSize { get; set; }
}