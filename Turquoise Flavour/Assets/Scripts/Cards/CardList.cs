using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using System.IO; 

namespace Turquoise
{
    public enum ECard
    {
        Slash,                          // 0
        Overheat,
        SteamJet,
        BuffArmor,
        BuffDamage,
        Discard,                        // 5
        ExhaustSelf,
        ExhaustOther,
        PedroHeal,
        BloodBite,
        IfBleedsDeals10,                // 10
        Find3Draw1,
        Slam,
        DualClaws,
        Bite,
        VampiricBite,                   // 15
        QuickAttack,
        DeepWounds,
        HeAttacks,
        HeProtects,
        OpenWounds,                     // 20
        Howl,
        StunningGrowl,
        Woof,
        ScareAway,
        ShakeItOff,                     // 25
        GoFetch,
        LickWounds,
        FindWeakness,
        DefensiveStance,
        HealthPotion,                   // 30
        FlameJet,
        FastAsLightning,
        ElectricTackle,
        StaticBite,
        FieryBite,                      // 35
        ChillingBite,
        RagingBite,
        FrenziedBite,
        EtherealForm,
        CloneCard,                      //40
        UnstableHeat,
        SurprisingShock,
        StaticShock,
        ThunderStrike,
        LightningBolt,                  //45
        MultiShock,
        ElectricSlash,
        Discharge,
        ConcentrateCharges,
        Grounding,
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

    public ECard GetRandomCard()
    {
        int i = Random.Range(0, (int)ECard.Count);
        return (ECard)i;
    }
}

[System.Serializable]
public struct CardDataTuplet
{
    public ECard cardName;
    public CardData cardData;
}
