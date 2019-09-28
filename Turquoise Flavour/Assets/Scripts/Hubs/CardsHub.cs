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
    public List<GameObject> m_child = new List<GameObject>();
    CardDetailsPanel m_detailsPanel;
    CardPanelUI m_currentlySelectedPanel;

    public void DisplayCardsForCreature(CreatureData data)
    {
        Reset();
        gameObject.SetActive(true);
        //TODO: Change creature 
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
            m_child.Add(cardPanel.gameObject);
            CardData cardData = GameMaster.GetInstance().m_cardList.GetCardDataFromCardName(card);

            bool owned = TheUnlocker.GetInstance().m_unlockedCards.Contains(card);
            int price = GetPricePerCardRarity(cardData.rarity);
            bool canBuy = price <= InventoryManager.GetInstance().GetPlayerGold();
            cardPanel.AssignCardData(cardData, true, owned, canBuy);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        //m_creatureData = null;
        m_image.sprite = null;
        m_creatureCards = null;
        foreach (var child in m_child)
        {
            Destroy(child.gameObject);
        }
        Destroy(m_detailsPanel?.gameObject);
    }

    private void Start()
    {
        DisplayCardsForCreature(null);
    }

    public static int GetPricePerCardRarity(Turquoise.ERarity rarity)
    {
        switch (rarity)
        {
            case Turquoise.ERarity.Common:
                return 5;
            case Turquoise.ERarity.Rare:
                return 10;
            case Turquoise.ERarity.Epic:
                return 20;
        }
        return 0;
    }

    public void OnCardPanelClicked(CardPanelUI cardPanel, int index)
    {
        if (m_currentlySelectedPanel == cardPanel)
        {
            Shrink();
        }
        else
        {
            m_currentlySelectedPanel = cardPanel;
            Expand(index);
            m_detailsPanel.InstantiateCardDetails(cardPanel);
        }
    }

    public void OnBuyCardClicked(CardPanelUI cardPanel)
    {
        TheUnlocker.GetInstance().UnlockCard(cardPanel.GetCardData().cardEnumValue);
        DisplayCardsForCreature(m_creatureData);
    }

    void Shrink()
    {
        if (m_detailsPanel != null)
        {
            Destroy(m_detailsPanel.gameObject);
        }
        m_currentlySelectedPanel = null;
    }

    void Expand(int index)
    {
        if (m_detailsPanel != null)
        {
            Destroy(m_detailsPanel.gameObject);
        }
        m_detailsPanel = Instantiate(m_cardDetailsPrefab, m_scrollableTransform).GetComponent<CardDetailsPanel>();
        m_detailsPanel.transform.SetSiblingIndex(index + 1);
    }
}
