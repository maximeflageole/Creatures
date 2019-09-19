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

    public void AssignCardData(CardData cardData, bool display = true)
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
    }

    public void Display()
    {
        enabled = true;
        m_cardImage.sprite = m_sprite;
        m_cardNameTextMesh.text = m_name;
        m_cardEnergyTextMesh.text = m_energy.ToString();
    }
}
