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

    public void InstantiateCardDetails(string text, bool canBuy)
    {
        m_text.text = text;
        m_unlockBtn.gameObject.SetActive(canBuy);
        m_priceText.gameObject.SetActive(canBuy);
        m_currencyImage.gameObject.SetActive(canBuy);
    }
}
