using AkashaRecords.Core.Enums;

namespace AkashaRecords.Core.Entities;

/// <summary>Arme. Donnée de jeu, référencée par <see cref="WeaponRecommendation"/>.</summary>
public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Rarity Rarity { get; set; }

    public int WeaponTypeId { get; set; }
    public WeaponType WeaponType { get; set; } = null!;

    public string? PassiveAbility { get; set; }
    public string? IconUrl { get; set; }
}