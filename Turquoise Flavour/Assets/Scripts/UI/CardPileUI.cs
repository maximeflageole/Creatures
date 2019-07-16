﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class CardPileUI : MonoBehaviour
{
    [SerializeField]
    protected List<CardData> m_cardsData;
    [SerializeField]
    protected List<GameObject> m_cardsGO;
    [SerializeField]
    protected GameObject m_cardUIPrefab;
    [SerializeField]
    protected int m_cardsToPick;
    [SerializeField]
    protected RectTransform m_cardsAnchorTransform;
    [SerializeField]
    protected List<CardUI> m_selectedCards;

    public void DisplayCardPile(List<ECard> cardList, bool orderAlpha, int cardsToPick)
    {
        ClearCards();
        m_cardsToPick = cardsToPick;
        foreach (var card in cardList)
        {
            AddCard(card);
        }
        DisplayCards(orderAlpha);
    }

    public void DisplayCardPile(List<Card> cardList, bool orderAlpha, int cardsToPick)
    {
        ClearCards();
        m_cardsToPick = cardsToPick;
        foreach (var card in cardList)
        {
            AddCard(card);
        }
        DisplayCards(orderAlpha);
    }

    public void ClearCards()
    {
        foreach (var card in m_cardsGO)
        {
            Destroy(card.gameObject);
        }
        m_cardsGO.Clear();
        m_cardsData.Clear();
        gameObject.SetActive(false);
        m_selectedCards.Clear();
    }

    void DisplayCards(bool orderAlpha)
    {
        List<CardData> cardsToShow = m_cardsData;
        if (orderAlpha)
        {
            cardsToShow = OrganizeAlpha(m_cardsData);
        }
        for (int i = 0; i < cardsToShow.Count; i++)
        {
            m_cardsGO[i].GetComponent<CardUI>().InitCardUI2D(cardsToShow[i]);
        }
        gameObject.SetActive(true);
    }

    void AddCard(ECard card)
    {
        GameObject cardInstance = Instantiate(m_cardUIPrefab, m_cardsAnchorTransform);
        m_cardsGO.Add(cardInstance);
        CardData data = GameMaster.GetInstance().m_cardList.GetCardDataFromCardName(card);
        m_cardsData.Add(data);
    }

    void AddCard(Card card)
    {
        GameObject cardInstance = Instantiate(m_cardUIPrefab, m_cardsAnchorTransform);
        m_cardsGO.Add(cardInstance);
        CardData data = card.GetCardData();
        m_cardsData.Add(data);
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

    public void OnCardSelect(CardUI cardUI)
    {
        if (!m_selectedCards.Contains(cardUI) && m_selectedCards.Count < m_cardsToPick)
        {
            m_selectedCards.Add(cardUI);
            cardUI.OnCardSelected(true);
        }
        else if (m_selectedCards.Contains(cardUI))
        {
            m_selectedCards.Remove(cardUI);
            cardUI.OnCardSelected(false);
        }
    }

    public void OnButtonSelect()
    {
        List<int> indexList = new List<int>();
        if (m_selectedCards.Count == m_cardsToPick)
        {
            foreach(var card in m_selectedCards)
            {
                indexList.Add(m_selectedCards.IndexOf(card));
            }
        }
        CardEffects.GetInstance().CardSelectionCallback(indexList, m_cardsToPick);
    }

    public bool Validate()
    {
        return false;
    }
}
