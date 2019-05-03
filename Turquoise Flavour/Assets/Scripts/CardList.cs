using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

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

public static class CardList
{
    public static string cardResourcesFolder = "Assets/Resources/Cards/";

    public static string GetCardAssetPath(ECard card)
    {
        return (cardResourcesFolder + card.ToString());
    }
}
