namespace AkashaRecords.Core.Exceptions;

/// <summary>Ressource demandée introuvable. Mappée en HTTP 404 par le middleware.</summary>
public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }

    /// <summary>Ex. : <c>new NotFoundException("Character", 5)</c> → « Character (id 5) est introuvable. »</summary>
    public NotFoundException(string entity, object key)
        : base($"{entity} (id {key}) est introuvable.") { }
}