using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsHub : MonoBehaviour
{
    public Image m_image;
    public CreatureData m_creatureData;
    public List<Turquoise.ECard> m_creatureCards;
    public GameObject m_cardPanelPrefab;
    public GameObject m_cardDetailsPrefab;
    public Transform m_scrollableTransform;

    public void DisplayCardsForCreature(CreatureData data)
    {
        Reset();
        gameObject.SetActive(true);
        //m_creatureData = data;
        m_image.sprite = m_creatureData.sprite;
        m_creatureCards = m_creatureData.abilityTree.commonCards;
        InstantiateCards();
    }

    public void InstantiateCards()
    {
        foreach(var card in m_creatureCards)
        {
            var cardPanel = Instantiate(m_cardPanelPrefab, m_scrollableTransform).GetComponent<CardPanelUI>();
            CardData cardData = GameMaster.GetInstance().m_cardList.GetCardDataFromCardName(card);
            bool owned = TheUnlocker.GetInstance().m_unlockedCards.Contains(card);
            cardPanel.AssignCardData(cardData, true, owned);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        //m_creatureData = null;
        m_image.sprite = null;
        m_creatureCards = null;
    }

    private void Start()
    {
        DisplayCardsForCreature(null);
    }
}
