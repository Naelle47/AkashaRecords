namespace AkashaRecords.Core.Entities;

/// <summary>
/// Set d'artefacts recommandé par un guide. Gère le 4pc (une ligne, <see cref="ComboGroup"/> null)
/// et le 2+2 (deux lignes partageant un même <see cref="ComboGroup"/>).
/// Règle domaine : la somme des <see cref="PieceCount"/> d'un combo vaut 4.
/// </summary>
public class ArtifactSetRecommendation
{
    public int Id { get; set; }

    public int BuildGuideId { get; set; }
    public BuildGuide BuildGuide { get; set; } = null!;

    public int ArtifactSetId { get; set; }
    public Artifact ArtifactSet { get; set; } = null!;

    public short PieceCount { get; set; }
    public short? ComboGroup { get; set; }
    public short Priority { get; set; }

    public short? TierId { get; set; }
    public RecommendationTier? Tier { get; set; }

    public string? Note { get; set; }
}