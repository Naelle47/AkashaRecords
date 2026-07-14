using AkashaRecords.Core.Enums;
using System.Xml.Linq;

namespace AkashaRecords.Core.Entities;

/// <summary>
/// Personnage jouable. Données de jeu (lore + gameplay), distinctes des guides de build.
/// L'élément et la région sont optionnels (ex. Voyageur) ; le type d'arme est obligatoire.
/// </summary>
public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Rarity Rarity { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public string? IconUrl { get; set; }

    public int? ElementId { get; set; }
    public Element? Element { get; set; }

    public int WeaponTypeId { get; set; }
    public WeaponType WeaponType { get; set; } = null!;

    public int? RegionId { get; set; }
    public Region? Region { get; set; }

    /// <summary>Guides de build associés à ce personnage.</summary>
    public ICollection<BuildGuide> BuildGuides { get; set; } = new List<BuildGuide>();
}