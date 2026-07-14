namespace AkashaRecords.Core.Entities;

/// <summary>
/// Rang d'une recommandation (BIS, HIGH, MEDIUM, LOW, SITUATIONAL, F2P, CRAFTABLE, EVENT). Référentiel.
/// </summary>
public class RecommendationTier
{
    public short Id { get; set; }
    public string Name { get; set; } = null!;
    public short SortOrder { get; set; }
}