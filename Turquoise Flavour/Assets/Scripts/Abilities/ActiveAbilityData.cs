using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active", menuName = "Active")]
public class ActiveAbilityData : ScriptableObject
{
    public string activeName;
    public string description;
    public Sprite artwork;
    public Sprite ManaCostArtwork;
    public Sprite CooldownArtwork;
    public int manaCost;
    public Cards.ETarget targetType;
    public List<SAbilityEffect> effects;
    public bool repeatable;
    public int cooldown;
}
