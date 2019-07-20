using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Turquoise.ECard cardEnumValue;
    public string description;
    public Sprite artwork;
    public int manaCost;
    public Turquoise.ETarget targetType;
    public List<SAbilityEffect> effects;
    public Turquoise.ECardGenre cardGenre;
    public Turquoise.EDamageType damageType;
    public Turquoise.ERarity rarity;
    public Turquoise.ECreaturePart creaturePart;
}