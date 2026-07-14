namespace AkashaRecords.Core.Exceptions;

/// <summary>
/// Conflit avec l'état courant : doublon, contrainte d'unicité violée
/// (ex. deux fois la même arme dans un guide). Mappée en HTTP 409 par le middleware.
/// </summary>
public sealed class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}