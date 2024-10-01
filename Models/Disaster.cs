using System.Diagnostics;



[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class Disaster
{
    public int Id { get; set; }
    public string DisasterType { get; set; }
    public string Location { get; set; }
    public DateTime DateOccurred { get; set; }
    public string Description { get; set; }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
