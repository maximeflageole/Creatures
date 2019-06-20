using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

[CreateAssetMenu(fileName = "New Ability Tree", menuName = "AbilityTree")]
public class AbilityTreeData : ScriptableObject
{
    public List<ERewardType> abilities;
    public List<ECard> commonCards;
    public List<ECard> rareCards;
    public List<ECard> epicCards;
    public List<ECard> legendaryCards;

    public List<ECard> GetCardsByRarity(ERarity rarity, int amount, bool noDuplicates = true)
    {
        List<ECard> cardList = new List<ECard>();
        List<ECard> cardsDrawn = new List<ECard>();
        switch (rarity)
        {
            case ERarity.Common:
                cardList = commonCards;
                break;
            case ERarity.Rare:
                cardList = rareCards;
                break;
            case ERarity.Epic:
                cardList = epicCards;
                break;
            case ERarity.Legendary:
                cardList = legendaryCards;
                break;
            default:
                break;
        }
        if (cardList != null)
        {
            int i = 0;
            while (i < amount)
            {
                cardsDrawn.Add(cardList[Random.Range(0, cardList.Count)]);
                i++;
            }
        }
        return cardsDrawn;
    }
}