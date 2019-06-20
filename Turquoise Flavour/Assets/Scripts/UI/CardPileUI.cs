﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class CardPileUI : MonoBehaviour
{
    [SerializeField]
    protected List<ECard> m_cards;
    [SerializeField]
    protected List<CardData> m_cardsData;
    [SerializeField]
    protected List<GameObject> m_cardsGO;
    [SerializeField]
    protected GameObject m_cardUIPrefab;

    public void DisplayCardPile(List<ECard> cardList, bool orderAlpha)
    {
        m_cardsGO.Clear();
        foreach (var card in cardList)
        {
            AddCard(card);
        }
        List<CardData> cardsToShow = m_cardsData;
        if (orderAlpha)
        {
            cardsToShow = OrganizeAlpha(m_cardsData);
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            m_cardsGO[i].GetComponent<CardUI>().InitCardUI2D(cardsToShow[i].cardName, cardsToShow[i].description, cardsToShow[i].manaCost.ToString(), cardsToShow[i].artwork);
        }
    }

    void AddCard(ECard card)
    {
        GameObject cardInstance = Instantiate(m_cardUIPrefab, transform);
        m_cardsGO.Add(cardInstance);
        CardData data = GameMaster.GetInstance().m_cardList.GetCardDataFromCardName(card);
        m_cardsData.Add(data);
    }

    public void Start()
    {
        DisplayCardPile(m_cards, false);
    }

    public List<CardData> OrganizeAlpha(List<CardData> initialCardList)
    {
        List<CardData> organizedList = new List<CardData>();

        while (initialCardList.Count != 0)
        {
            CardData firstAlpha = initialCardList[0];

            for (int i = 0; i < initialCardList.Count; i++)
            {
                if (firstAlpha.cardName.CompareTo(initialCardList[i].cardName) > 0)
                {
                    firstAlpha = initialCardList[i];
                }
            }
            organizedList.Add(firstAlpha);
            initialCardList.Remove(firstAlpha);
        }
        return organizedList;
    }
}
