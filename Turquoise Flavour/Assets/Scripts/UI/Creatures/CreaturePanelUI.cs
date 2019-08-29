using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreaturePanelUI : MonoBehaviour
{
    [SerializeField]
    protected Creature m_creature;
    [SerializeField]
    protected Image m_image;
    [SerializeField]
    protected TextMeshProUGUI m_name;


    public void OnOpenMenu(Creature creature)
    {
        m_creature = creature;
        m_name.text = creature.GetName();
        m_image.sprite = creature.GetSprite();
    }
}
