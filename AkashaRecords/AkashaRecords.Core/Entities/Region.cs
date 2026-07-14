namespace AkashaRecords.Core.Entities;

/// <summary>Région/nation (Mondstadt, Liyue…). Référentiel. <see cref="SortOrder"/> ordonne l'affichage.</summary>
public class Region
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public short SortOrder { get; set; }
    public string? IconUrl { get; set; }
}