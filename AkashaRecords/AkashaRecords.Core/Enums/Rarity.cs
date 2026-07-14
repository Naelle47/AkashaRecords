using System;
using System.Collections.Generic;
using System.Text;

namespace AkashaRecords.Core.Enums;

/// <summary>
/// Rareté d'un personnage ou d'une arme, en nombre d'étoiles.
/// Stockée en <c>smallint</c> en base (décision : entier, pas d'enum natif Postgres) ;
/// l'enum est adossé à <see cref="byte"/> et mappe donc directement sur la valeur numérique.
/// Contraintes en base : personnages 4–5 étoiles, armes 1–5 étoiles (CHECK par table).
/// </summary>
public enum Rarity : byte
{
    OneStar = 1,
    TwoStar = 2,
    ThreeStar = 3,
    FourStar = 4,
    FiveStar = 5
}
