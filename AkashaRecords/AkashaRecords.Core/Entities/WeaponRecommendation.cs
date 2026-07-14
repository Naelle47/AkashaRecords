namespace AkashaRecords.Core.Entities;

/// <summary>Arme recommandée par un guide, avec priorité et rang optionnel (tier).</summary>
public class WeaponRecommendation
{
    public int Id { get; set; }

    public int BuildGuideId { get; set; }
    public BuildGuide BuildGuide { get; set; } = null!;

    public int WeaponId { get; set; }
    public Weapon Weapon { get; set; } = null!;

    public short Priority { get; set; }

    public short? TierId { get; set; }
    public RecommendationTier? Tier { get; set; }

    public string? Note { get; set; }
}