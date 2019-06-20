using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpList : MonoBehaviour
{
    [SerializeField]
    protected List<LevelUpTuples> LevelUpDataDictionnary;

    public static string levelUpResourcesFolder = "Assets/Resources/LevelUpAbilities/";

    public static string GetLevelUpAssetPath(Turquoise.ERewardType rewardType)
    {
        return (levelUpResourcesFolder + rewardType.ToString() + ".asset");
    }

    public LevelUpAbilityData GetLevelUpDataFromRewardType(Turquoise.ERewardType rewardType)
    {
        foreach (var levelUpTuplet in LevelUpDataDictionnary)
        {
            if (levelUpTuplet.rewardType == rewardType)
            {
                return levelUpTuplet.levelUpData;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct LevelUpTuples
{
    public Turquoise.ERewardType rewardType;
    public LevelUpAbilityData levelUpData;
}
