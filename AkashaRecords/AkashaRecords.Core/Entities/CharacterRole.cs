namespace AkashaRecords.Core.Entities;

/// <summary>Rôle de combat (Main DPS, Sub DPS, Support, Healer, Shield, Buffer, Driver). Référentiel.</summary>
public class CharacterRole
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}