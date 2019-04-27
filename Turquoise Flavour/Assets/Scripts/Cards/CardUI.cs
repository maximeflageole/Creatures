using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshPro m_nameText;
    [SerializeField]
    protected SpriteRenderer m_image;
    [SerializeField]
    protected TextMeshPro m_descriptionText;
    [SerializeField]
    protected TextMeshPro m_manaText;


    public void InitCardUI(string name, string description, string mana, Sprite image)
    {
        m_nameText.text = name;
        m_descriptionText.text = description;
        m_manaText.text = mana;
        m_image.sprite = image;
    }
}
