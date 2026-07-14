namespace AkashaRecords.Core.Entities;

/// <summary>
/// Rôle porté par un guide, avec priorité (multi-rôle ordonné : ex. Sub DPS(1) + Support(2)).
/// Invariant : une seule priorité 1 par guide.
/// </summary>
public class BuildRole
{
    public int Id { get; set; }

    public int BuildGuideId { get; set; }
    public BuildGuide BuildGuide { get; set; } = null!;

    public int RoleId { get; set; }
    public CharacterRole Role { get; set; } = null!;

    public short Priority { get; set; }
}