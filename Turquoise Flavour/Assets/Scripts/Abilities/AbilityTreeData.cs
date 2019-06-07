using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Tree", menuName = "AbilityTree")]
public class AbilityTreeData : ScriptableObject
{
    public List<LevelUpData> levelUpData;
}

[System.Serializable]
public struct LevelUpData
{
    public int level;
    public List<LevelUpAbilityData> levelAbilities;
}
