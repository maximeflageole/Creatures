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
    [SerializeField]
    protected RectTransform m_healthBar;
    [SerializeField]
    protected float m_healthBarMinValue;
    [SerializeField]
    protected float m_healthBarMaxValue;

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
        PositionHealthBar();
    }

    public void OnClick()
    {
        GetComponentInParent<CreaturesPanelUI>().OnSubMenuClicked(m_creature);
    }

    void PositionHealthBar()
    {
        float positionX = m_healthBarMinValue * (1.0f - m_creature.GetHealthRatio()) + (m_healthBarMaxValue *m_creature.GetHealthRatio());
        Debug.Log("Position = " + positionX + " Health ratio = " + m_creature.GetHealthRatio());
        m_healthBar.localPosition = new Vector3(positionX, 0, 0);
    }
}