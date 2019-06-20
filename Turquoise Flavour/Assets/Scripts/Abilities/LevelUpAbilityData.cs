using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Level Up Ability", menuName = "LevelUpAbility")]
public class LevelUpAbilityData : ScriptableObject
{
    public Sprite sprite;
    public string text;
    public Turquoise.ERewardType rewardType;
}
