namespace MASZ.Punishments.Models;

public class CaseTable
{
	public CaseTable(List<ModCaseTableEntry> modCase, int fullSize)
	{
		Cases = modCase;
		FullSize = fullSize;
	}

	public List<ModCaseTableEntry> Cases { get; set; }
	public int FullSize { get; set; }
}