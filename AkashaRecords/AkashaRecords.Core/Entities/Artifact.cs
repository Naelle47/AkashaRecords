namespace AkashaRecords.Core.Entities;

/// <summary>
/// Set d'artefacts (la table stocke des sets, avec leurs bonus 2 et 4 pièces).
/// Référencé par <see cref="ArtifactSetRecommendation"/>.
/// </summary>
public class Artifact
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Bonus2Pc { get; set; }
    public string? Bonus4Pc { get; set; }
    public string? IconUrl { get; set; }
}