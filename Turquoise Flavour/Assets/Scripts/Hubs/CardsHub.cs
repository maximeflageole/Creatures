using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Image m_currencyImage;
    public TextMeshProUGUI m_currencyText;
    int creatureIndex = 0;

    public void OpenMenu()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Reset();
            m_creatureData = null;
            return;
        }
        gameObject.SetActive(true);
        DisplayCardsForCreature(GameMaster.GetInstance().m_creatureList.GetAllCreaturesInDexOrder()[creatureIndex]);
    }

    public void DisplayCardsForCreature(CreatureData data)
    {
        Reset();
        gameObject.SetActive(true);
        m_creatureData = data;
        m_image.sprite = m_creatureData.sprite;
        m_creatureCards = m_creatureData.abilityTree.commonCards;
        InstantiateCards();
        m_currencyImage.sprite = data.sprite;
        m_currencyText.text = "x " + InventoryManager.GetInstance().GetCreatureCurrency(data.eCreature).ToString();
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
            bool canBuy = InventoryManager.GetInstance().CanBuyWithCreatureCurrency(m_creatureData.eCreature, price);
            cardPanel.AssignCardData(cardData, true, owned, canBuy);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        m_image.sprite = null;
        m_creatureCards = null;
        foreach (var child in m_child)
        {
            Destroy(child.gameObject);
        }
        if (m_detailsPanel != null)
        {
            Destroy(m_detailsPanel.gameObject);
        }
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
        if (InventoryManager.GetInstance().TryBuyWithCreatureCurrency(m_creatureData.eCreature, GetPricePerCardRarity(cardPanel.GetCardData().rarity)))
        {
            TheUnlocker.GetInstance().UnlockCard(cardPanel.GetCardData().cardEnumValue);
            DisplayCardsForCreature(m_creatureData);
        }
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

    public void GetNextCreature(int iteration)
    {
        creatureIndex += iteration;
        var creatures = GameMaster.GetInstance().m_creatureList.GetAllCreaturesInDexOrder();
        creatureIndex %= creatures.Count;
        if (creatureIndex < 0)
        {
            creatureIndex += creatures.Count;
        }
        DisplayCardsForCreature(creatures[creatureIndex]);
    }
}
