using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

[CreateAssetMenu(fileName = "New Explorator", menuName = "Explorator")]
public class ExploratorData : ScriptableObject
{
    public EExplorator explorator;
    public Sprite sprite;
}

namespace Turquoise
{
    public enum EExplorator
    {
        Biologist,
        Engineer,
        Tanner,
        Smith,
        Jeweler,
        Captain,
        Trader,
        Alchemist,
        Beastmaster,
        Count
    }
}