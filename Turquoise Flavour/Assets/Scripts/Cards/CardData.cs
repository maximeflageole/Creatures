﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int manaCost;
    public List<SCardEffect> effects;
    public Cards.ECardType cardType;
    public Cards.ERarity rarity;
    public Cards.ECreaturePart creaturePart;
}