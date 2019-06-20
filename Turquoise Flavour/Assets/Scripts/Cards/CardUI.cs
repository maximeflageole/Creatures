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


    public void InitCardUI(string name, string description, string mana, Sprite sprite)
    {
        m_nameText.text = name;
        m_descriptionText.text = description;
        m_manaText.text = mana;
        m_sprite.sprite = sprite;
    }

    public void InitCardUI2D(string name, string description, string mana, Sprite sprite)
    {
        m_nameText2D.text = name;
        m_descriptionText2D.text = description;
        m_manaText2D.text = mana;
        m_image2D.sprite = sprite;
    }
}
