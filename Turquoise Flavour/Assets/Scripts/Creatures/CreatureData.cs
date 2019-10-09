using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature")]
public class CreatureData : ScriptableObject
{
    public ECreature eCreature;
    public string creatureName;
    public ECreatureType creatureType;
    public int initialHealth;
    public int healthPerLevel;
    public int initialMana;
    public Sprite sprite;
    public List<Turquoise.ECard> baseDeck;
    public ActiveAbilityData activeAbilityData;
    public Experience.ELevelSpeed levelSpeed;
    public AbilityTreeData abilityTree;
    public CreatureEvolutionTree evolutionTree;
}