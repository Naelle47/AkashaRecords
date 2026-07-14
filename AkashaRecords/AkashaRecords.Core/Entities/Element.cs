namespace AkashaRecords.Core.Entities;

/// <summary>Élément Genshin (Pyro, Hydro, Anemo…). Référentiel.</summary>
public class Element
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? IconUrl { get; set; }
}