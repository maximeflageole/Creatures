using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshPro m_nameText;
    [SerializeField]
    protected SpriteRenderer m_sprite;
    [SerializeField]
    protected TextMeshPro m_descriptionText;
    [SerializeField]
    protected TextMeshPro m_manaText;
    [SerializeField]
    protected TextMeshProUGUI m_nameText2D;
    [SerializeField]
    protected Image m_image2D;
    [SerializeField]
    protected TextMeshProUGUI m_descriptionText2D;
    [SerializeField]
    protected TextMeshProUGUI m_manaText2D;
    [SerializeField]
    protected CardData m_cardData;
    public CardData GetCardData() { return m_cardData; }


    public void InitCardUI(string name, string description, string mana, Sprite sprite)
    {
        m_nameText.text = name;
        m_descriptionText.text = description;
        m_manaText.text = mana;
        m_sprite.sprite = sprite;
    }

    public void InitCardUI2D(CardData cardData)
    {
        m_cardData = cardData;
        m_nameText2D.text = cardData.cardName;
        m_descriptionText2D.text = cardData.description;
        m_manaText2D.text = cardData.energyCost.ToString();
        m_image2D.sprite = cardData.artwork;
    }

    public void OnClick()
    {
        var cardPileUI = GetComponentInParent<CardPileUI>();
        if (cardPileUI != null)

        {
            cardPileUI.OnCardSelect(this);
        }
    }

    public void OnCardSelected(bool selected)
    {
        if (selected)
            transform.localScale *= 1.1f;
        else
            transform.localScale /= 1.1f;
    }
}
