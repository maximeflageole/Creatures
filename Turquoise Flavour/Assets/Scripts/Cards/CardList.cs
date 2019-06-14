using System.Collections;
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
        BuffArmor,
        BuffDamage,
        Discard,
        ExhaustSelf,
        ExhaustOther,
        PedroHeal,
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
    public Cards.ECard cardName;
    public CardData cardData;
}
