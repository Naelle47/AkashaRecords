using AkashaRecords.Core.Enums;

namespace AkashaRecords.Core.Entities;

/// <summary>
/// Statistique référencée par les recommandations (ATQ%, CRIT Rate, Elemental Mastery…).
/// <see cref="Type"/> distingue valeur fixe et pourcentage.
/// </summary>
public class Stat
{
    public short Id { get; set; }
    public string Name { get; set; } = null!;
    public StatType Type { get; set; }
}