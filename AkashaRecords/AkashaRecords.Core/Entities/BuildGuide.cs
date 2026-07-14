namespace AkashaRecords.Core.Entities;

/// <summary>
/// Guide de build : racine d'agrégat. Assemble, pour un personnage, les recommandations
/// (rôles, armes, sets d'artefacts, main/sub stats). Un guide est une ligne directrice
/// d'optimisation ; sa version de jeu est indicative et optionnelle.
/// </summary>
public class BuildGuide
{
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short? GameVersionId { get; set; }
    public GameVersion? GameVersion { get; set; }

    public string? Summary { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // Recommandations possédées par le guide (cycle de vie lié : ON DELETE CASCADE).
    public ICollection<BuildRole> Roles { get; set; } = new List<BuildRole>();
    public ICollection<WeaponRecommendation> WeaponRecommendations { get; set; } = new List<WeaponRecommendation>();
    public ICollection<ArtifactSetRecommendation> ArtifactSetRecommendations { get; set; } = new List<ArtifactSetRecommendation>();
    public ICollection<MainStatRecommendation> MainStatRecommendations { get; set; } = new List<MainStatRecommendation>();
    public ICollection<SubStatRecommendation> SubStatRecommendations { get; set; } = new List<SubStatRecommendation>();
}