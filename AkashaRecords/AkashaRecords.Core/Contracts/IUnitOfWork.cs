using AkashaRecords.Core.Contracts.Repositories;

namespace AkashaRecords.Core.Contracts;

/// <summary>
/// Unité de travail : propriétaire d'UNE transaction et des repos qui la partagent.
/// Représente une seule opération métier transactionnelle.
///
/// Pour exécuter plusieurs transactions en parallèle, on ne réutilise pas la même
/// instance : on en crée plusieurs via <see cref="IUnitOfWorkFactory"/>, chacune avec
/// sa propre connexion — c'est ce qui garantit l'isolation entre transactions concurrentes.
///
/// Usage type : Begin → écritures via les repos → Commit (ou Rollback en cas d'erreur).
/// La libération (<see cref="IAsyncDisposable"/>) fait un rollback si aucun commit n'a eu lieu.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>Repository de l'agrégat BuildGuide, lié à la transaction de cette UoW.</summary>
    IBuildGuideRepository BuildGuides { get; }

    /// <summary>Ouvre la transaction. À appeler avant toute écriture.</summary>
    Task BeginAsync(CancellationToken cancellationToken = default);

    /// <summary>Valide définitivement les écritures de la transaction.</summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>Annule toutes les écritures de la transaction (rollback).</summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}