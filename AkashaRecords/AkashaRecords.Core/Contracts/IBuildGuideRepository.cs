using AkashaRecords.Core.Entities;

namespace AkashaRecords.Core.Contracts.Repositories;

/// <summary>
/// Repository d'écriture de l'agrégat <see cref="BuildGuide"/>.
/// La racine est manipulée avec ses recommandations (rôles, armes, sets, main/sub stats),
/// dont le cycle de vie lui est lié.
/// </summary>
public interface IBuildGuideRepository : IRepository<BuildGuide>
{
    /// <summary>
    /// Charge le guide complet — racine + recommandations — pour lecture-en-vue-d'écriture.
    /// Renvoie <c>null</c> si le guide n'existe pas.
    /// </summary>
    Task<BuildGuide?> GetAggregateAsync(int id, CancellationToken cancellationToken = default);
}