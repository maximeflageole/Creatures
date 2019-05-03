﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using System.IO; 

namespace Cards
{
    public enum ECard
    {
        Slash,
        Overheat,
        SteamJet,
        Count
    }
}

public class CardList: MonoBehaviour
{
    [SerializeField]
    protected List<CardDataDictionnary> cardDataDictionnary;

    public static string cardResourcesFolder = "Assets/Resources/Cards/";

    public static string GetCardAssetPath(ECard card)
    {
        return (cardResourcesFolder + card.ToString() + ".asset");
    }

    public CardData GetCardDataFromCardName(ECard card)
    {
        foreach (var cardTuplet in cardDataDictionnary)
        {
            if (cardTuplet.cardName == card)
            {
                return cardTuplet.cardData;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct CardDataDictionnary
{
    public Cards.ECard cardName;
    public CardData cardData;
}
