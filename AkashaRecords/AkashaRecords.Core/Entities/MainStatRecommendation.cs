namespace AkashaRecords.Core.Entities;

/// <summary>
/// Main stat recommandée pour un emplacement (Sands / Goblet / Circlet), avec priorité.
/// Invariant : une seule priorité 1 par (guide, slot).
/// </summary>
public class MainStatRecommendation
{
    public int Id { get; set; }

    public int BuildGuideId { get; set; }
    public BuildGuide BuildGuide { get; set; } = null!;

    public short SlotId { get; set; }
    public ArtifactSlotType Slot { get; set; } = null!;

    public short StatId { get; set; }
    public Stat Stat { get; set; } = null!;

    public short Priority { get; set; }
}