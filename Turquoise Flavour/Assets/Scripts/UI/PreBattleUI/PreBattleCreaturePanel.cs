using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreBattleCreaturePanel : MonoBehaviour
{
    public Image m_image;
    public Button m_btn1;
    public Button m_btn2;
    public CreatureData m_creatureData;
    protected PreBattle m_preBattleRef;

    public void AssignCreature(CreatureData data)
    {
        Reset();
        if (data != null)
        {
            m_creatureData = data;
            m_image.sprite = data.sprite;
            gameObject.SetActive(true);
        }
    }

    void Reset()
    {
        m_creatureData = null;
        m_image.sprite = null;
        gameObject.SetActive(false);
    }

    public void OnClickCard1()
    {
        var items = InventoryManager.GetInstance().GetInventoryItemsDataForType(Turquoise.EItemTypes.Consumables);
        if (items.Count != 0)
        {
            Carousel carousel = Instantiate(GetPreBattle().m_carouselPrefab, m_btn1.transform).GetComponent<Carousel>();
            carousel.AssignElements(items);
            m_btn1.enabled = false;
        }
    }

    public void OnClickCard2()
    {
        var items = InventoryManager.GetInstance().GetInventoryItemsDataForType(Turquoise.EItemTypes.Consumables);
        if (items.Count != 0)
        {
            Carousel carousel = Instantiate(GetPreBattle().m_carouselPrefab, m_btn2.transform).GetComponent<Carousel>();
            carousel.AssignElements(items);
            m_btn2.enabled = false;
        }
    }

    PreBattle GetPreBattle()
    {
        if (m_preBattleRef != null)
            return m_preBattleRef;
        return GetComponentInParent<PreBattle>();
    }
}
