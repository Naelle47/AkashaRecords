namespace AkashaRecords.Core.Entities;

/// <summary>
/// Emplacement d'artefact (Flower, Plume, Sands, Goblet, Circlet). Référentiel.
/// Les main stats ne concernent que Sands / Goblet / Circlet (Flower = PV, Plume = ATQ, fixes).
/// </summary>
public class ArtifactSlotType
{
    public short Id { get; set; }
    public string Name { get; set; } = null!;
    public short SortOrder { get; set; }
}