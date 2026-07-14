using System;
using System.Collections.Generic;
using System.Text;

namespace AkashaRecords.Core.Enums;

/// <summary>
/// Nature d'une statistique : valeur fixe ou pourcentage.
/// Distingue par exemple ATQ fixe (<see cref="Flat"/>) de ATQ% (<see cref="Percent"/>).
/// En base : colonne <c>stats.stat_type</c>, CHECK ('FLAT','PERCENT').
/// Le mapping vers/depuis la chaîne SQL est géré côté Infrastructure.
/// </summary>
public enum StatType : byte
{
    Flat = 1,
    Percent = 2
}
