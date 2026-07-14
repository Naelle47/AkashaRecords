namespace AkashaRecords.Core.Entities;

/// <summary>Sub stat recommandée par un guide, avec priorité.</summary>
public class SubStatRecommendation
{
    public int Id { get; set; }

    public int BuildGuideId { get; set; }
    public BuildGuide BuildGuide { get; set; } = null!;

    public short StatId { get; set; }
    public Stat Stat { get; set; } = null!;

    public short Priority { get; set; }
}