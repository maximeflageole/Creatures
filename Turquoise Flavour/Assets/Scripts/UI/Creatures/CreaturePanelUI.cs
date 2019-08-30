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
    [SerializeField]
    protected RectTransform m_childTransform;

    public void Reset()
    {
        m_childTransform.gameObject.SetActive(false);
        m_creature = null;
    }

    public void OnOpenMenu(Creature creature)
    {
        m_childTransform.gameObject.SetActive(true);
        m_creature = creature;
        m_name.text = creature.GetName();
        m_image.sprite = creature.GetSprite();
    }

    public void OnClick()
    {
        GetComponentInParent<CreaturesPanelUI>().OnSubMenuClicked(m_creature);
    }
}