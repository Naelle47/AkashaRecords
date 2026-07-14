namespace AkashaRecords.Core.Entities;

/// <summary>Type d'arme (Sword, Claymore, Polearm, Bow, Catalyst). Référentiel.</summary>
public class WeaponType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? IconUrl { get; set; }
}