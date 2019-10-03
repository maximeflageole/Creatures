using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reward", menuName = "Reward")]
public class CreatureRewardData : ScriptableObject
{
    public List<sRewardOdds> rewards = new List<sRewardOdds>();
}

[System.Serializable]
public struct sRewardOdds
{
    public List<Turquoise.ERarity> itemRarities;
    public int oddsOnHundred;
    public Vector2 m_goldMinMax;
}