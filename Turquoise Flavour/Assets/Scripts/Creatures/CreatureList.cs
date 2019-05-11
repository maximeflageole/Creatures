using System.Collections;
using System.Collections.Generic;
using Creatures;
using UnityEngine;

public class CreatureList : MonoBehaviour
{
    [SerializeField]
    protected List<CreatureDataTuplet> creatureDataDictionnary;

    public static string creaturesResourcesFolder = "Assets/Resources/Creatures/";

    public static string GetCreatureAssetPath(ECreature creature)
    {
        return (creaturesResourcesFolder + creature.ToString() + ".asset");
    }

    public CreatureData GetCreatureDataFromCreatureName(ECreature creature)
    {
        foreach (var creatureTuplet in creatureDataDictionnary)
        {
            if (creatureTuplet.creatureName == creature)
            {
                return creatureTuplet.creatureData;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct CreatureDataTuplet
{
    public ECreature creatureName;
    public CreatureData creatureData;
}
