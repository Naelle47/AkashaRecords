namespace AkashaRecords.Core.Contracts;

/// <summary>
/// Fabrique d'unités de travail. Chaque appel crée une <see cref="IUnitOfWork"/> neuve,
/// avec sa propre connexion et sa propre transaction.
///
/// C'est le mécanisme prévu pour les transactions simultanées : deux opérations
/// concurrentes demandent chacune leur UoW à la fabrique et restent totalement isolées,
/// plutôt que de partager une connexion (source de bugs et de blocages).
/// </summary>
public interface IUnitOfWorkFactory
{
    /// <summary>Crée une nouvelle unité de travail prête à l'emploi (connexion ouverte).</summary>
    Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
}