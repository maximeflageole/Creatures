using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evolution tree", menuName = "Evolution tree")]
public class CreatureEvolutionTree : ScriptableObject
{
    public List<sEvolutionLevel> evolutionNodes;
}

[System.Serializable]
public struct sEvolutionNode
{
    public string description;
    public Sprite sprite;
    public SAbilityEffect abilityEffect;
}

[System.Serializable]
public struct sEvolutionLevel
{
    public int level;
    public sEvolutionNode nodeLeft;
    public sEvolutionNode nodeRight;
}