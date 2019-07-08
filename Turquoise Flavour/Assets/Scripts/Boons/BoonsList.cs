using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class BoonsList : MonoBehaviour
{
    [SerializeField]
    protected List<BoonsTuplet> boonsDictionnary;

    public static string boonsRessourcesFolder = "Assets/Resources/Boons/";

    public static string GetBoonAssetPath(EConditionType condition)
    {
        return (boonsRessourcesFolder + condition.ToString() + ".asset");
    }

    public ConditionData GetBoonDataFromCardEffect(ECardEffect condition)
    {
        foreach (var boonTuplet in boonsDictionnary)
        {
            if (boonTuplet.cardEffect == condition)
            {
                return boonTuplet.boonData;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct BoonsTuplet
{
    public ECardEffect cardEffect;
    public ConditionData boonData;
}