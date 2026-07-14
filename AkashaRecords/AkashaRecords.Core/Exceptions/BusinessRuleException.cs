namespace AkashaRecords.Core.Exceptions;

/// <summary>
/// Règle métier non respectée — typiquement les invariants non exprimables en SQL :
/// somme des pièces d'un combo d'artefacts ≠ 4, plusieurs priorités 1 dans un guide, etc.
/// Mappée en HTTP 400 par le middleware.
/// </summary>
public sealed class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message) { }
}