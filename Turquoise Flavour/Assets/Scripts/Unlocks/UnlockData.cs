using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

[CreateAssetMenu(fileName = "New Unlock", menuName = "Unlock")]
public class UnlockData : ScriptableObject
{
    public EStat stat;
    public EOperation operation;
    public int comparisonNumber;
    public EStat comparisonStat;
    public List<EExplorator> exploratorsUnlocks;
    public List<ECard> cardsUnlocks;
    public List<EItem> itemsUnlocks;
}

namespace Turquoise
{
    public enum EOperation
    {
        Equals,
        Bigger,
        Less,
        BiggerOrEqual,
        LessOrEqual,
        NotEqual,
        Count
    }
}
