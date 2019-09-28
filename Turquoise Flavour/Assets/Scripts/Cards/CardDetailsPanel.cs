using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDetailsPanel : MonoBehaviour
{
    public TextMeshProUGUI m_text;
    public Button m_unlockBtn;
    public TextMeshProUGUI m_priceText;
    public Image m_currencyImage;
    public CardPanelUI m_cardPanel;

    public void InstantiateCardDetails(CardPanelUI cardPanel)
    {
        m_cardPanel = cardPanel;
        CardData cardData = cardPanel.GetCardData();
        m_text.text = cardData.description;

        m_unlockBtn.gameObject.SetActive(cardPanel.m_canBuy);
        m_priceText.gameObject.SetActive(cardPanel.m_canBuy);
        m_currencyImage.gameObject.SetActive(cardPanel.m_canBuy);
        m_priceText.text = "x " + CardsHub.GetPricePerCardRarity(cardData.rarity).ToString();
    }

    public void OnClick()
    {
        GetComponentInParent<CardsHub>()?.OnBuyCardClicked(m_cardPanel);
    }
}
