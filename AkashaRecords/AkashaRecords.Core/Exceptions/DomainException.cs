namespace AkashaRecords.Core.Exceptions;

/// <summary>
/// Base de toutes les exceptions métier bloquantes du domaine.
/// Le middleware de l'application les traduit en codes HTTP (par type).
/// Le Core reste agnostique du transport : aucune référence HTTP ici.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}