namespace AkashaRecords.Core.Contracts;

/// <summary>
/// Contrat d'écriture générique pour une entité à clé entière.
/// Les opérations s'exécutent dans la transaction portée par l'<see cref="IUnitOfWork"/> courant :
/// un repo obtenu via une UoW partage sa connexion et sa transaction.
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>Charge une entité par son identifiant, ou <c>null</c> si absente.</summary>
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Insère l'entité et renvoie l'identifiant généré.</summary>
    Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Met à jour l'entité existante.</summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Supprime l'entité par son identifiant.</summary>
    Task RemoveAsync(int id, CancellationToken cancellationToken = default);
}