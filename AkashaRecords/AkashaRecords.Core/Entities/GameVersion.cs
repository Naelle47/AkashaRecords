namespace AkashaRecords.Core.Entities;

/// <summary>
/// Version de jeu (patch), ex. « 6.0 ». Référencée de façon optionnelle par un guide :
/// indique le patch que reflète le guide, sans le rendre obsolète.
/// </summary>
public class GameVersion
{
    public short Id { get; set; }
    public string Version { get; set; } = null!;
    public DateOnly? ReleaseDate { get; set; }
}