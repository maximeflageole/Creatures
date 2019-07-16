﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using System.IO; 

namespace Turquoise
{
    public enum ECard
    {
        Slash,
        Overheat,
        SteamJet,
        BuffArmor,
        BuffDamage,
        Discard,
        ExhaustSelf,
        ExhaustOther,
        PedroHeal,
        BloodBite,
        IfBleedsDeals10,
        Find3Draw1,
        Count
    }
}

public class CardList: MonoBehaviour
{
    [SerializeField]
    protected List<CardDataTuplet> cardDataDictionnary;

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
        Debug.Log("Card " + card + "could not be found in CardList! Are you sure you added it?");
        return null;
    }
}

[System.Serializable]
public struct CardDataTuplet
{
    public ECard cardName;
    public CardData cardData;
}
