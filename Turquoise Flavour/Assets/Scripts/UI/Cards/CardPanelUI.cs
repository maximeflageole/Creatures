using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPanelUI : MonoBehaviour
{
    [SerializeField]
    protected Image m_cardImage;
    [SerializeField]
    protected TextMeshProUGUI m_cardNameTextMesh;
    [SerializeField]
    protected TextMeshProUGUI m_cardEnergyTextMesh;
    [SerializeField]
    protected CardData m_cardData;
    [SerializeField]
    protected Sprite m_sprite;
    [SerializeField]
    protected string m_name;
    [SerializeField]
    protected string m_text;
    [SerializeField]
    protected int m_energy;
    public Color m_ownedColor;
    public Color m_availableColor;
    public Color m_tooExpensiveColor;


    public void AssignCardData(CardData cardData, bool display = true, bool owned = true, bool canbuy = true)
    {
        m_cardData = cardData;

        m_sprite = cardData.artwork;
        m_name = cardData.cardName;
        m_text = cardData.description;
        m_energy = cardData.energyCost;

        if (display)
        {
            Display();
        }
        else
        {
            enabled = false;
        }
        if (!owned)
        {
            if (canbuy)
            {
                GetComponent<Image>().color = m_availableColor;
            }
            else
            {
                GetComponent<Image>().color = m_tooExpensiveColor;
            }
        }
    }

    public void Display()
    {
        enabled = true;
        m_cardImage.sprite = m_sprite;
        m_cardNameTextMesh.text = m_name;
        m_cardEnergyTextMesh.text = m_energy.ToString();
    }
}
