using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using TMPro;

public class CreaturesPanelUI : TurquoisePanel
{
    [SerializeField]
    protected List<CreaturePanelUI> m_creaturePanels;
    [SerializeField]
    protected Creature m_currentCreature;
    [SerializeField]
    protected List<GameObject> m_child;
    [SerializeField]
    protected GameObject m_actionPanelUIPrefab;
    protected ActionPanelUI m_actionPanelUIInstance;
    [SerializeField]
    protected RectTransform m_panelTransform;
    [SerializeField]
    protected bool m_inCreatureSelection;
    protected EItem m_item = EItem.Count;

    public virtual void OpenMenu(List<Creature> creatures)
    {
        if (gameObject.activeSelf)
        {
            Reset();
            return;
        }
        Reset();
        gameObject.SetActive(true);
        for (int i = 0; i < creatures.Count && i < 3; i++)
        {
            m_creaturePanels[i].OnOpenMenu(creatures[i]);
        }
    }

    public override void Reset()
    {
        gameObject.SetActive(false);
        foreach (var creaturePanel in m_creaturePanels)
        {
            creaturePanel.Reset();
        }
        m_inCreatureSelection = false;
        if (m_actionPanelUIInstance != null)
        {
            Destroy(m_actionPanelUIInstance.gameObject);
            m_actionPanelUIInstance = null;
        }
    }

    public void StartSelectCreature()
    {
        m_inCreatureSelection = true;
        OpenMenu(Player.GetPlayerInstance().GetCreatures());
    }

    public void StartSelectCreatureForItem(EItem item)
    {
        m_inCreatureSelection = true;
        m_item = item;
        OpenMenu(Player.GetPlayerInstance().GetCreatures());
    }

    public void OnSubMenuClicked(Creature creature)
    {
        if (m_item != EItem.Count && m_inCreatureSelection)
        {
            var itemData = InventoryManager.GetInstance().GetItemFromEnum(m_item);
            switch (itemData.type)
            {
                case EItemTypes.Trinkets:
                    creature.EquipItem(m_item);
                    break;
                case EItemTypes.Consumables:
                    creature.UseItem(itemData);
                    break;
                case EItemTypes.TMs:
                    creature.GiveTM(itemData);
                    break;
                case EItemTypes.Neutral:
                    break;
                default:
                    break;
            }

            Reset();
            m_item = EItem.Count;
            return;
        }
        if (m_actionPanelUIInstance != null)
        {
            Destroy(m_actionPanelUIInstance.gameObject);
            m_actionPanelUIInstance = null;
        }
        if (creature != null)
        {
            m_actionPanelUIInstance = Instantiate(m_actionPanelUIPrefab, transform).GetComponent<ActionPanelUI>();
            m_actionPanelUIInstance.CreateCreatureMenu();
        }

        m_currentCreature = creature;
    }

    public void OnCreatureActionClicked(ECreatureInteraction creatureInteraction)
    {
        switch (creatureInteraction)
        {
            case ECreatureInteraction.Swap:
                Player.GetPlayerInstance().SwapCreature(m_currentCreature);
                break;
            case ECreatureInteraction.Use:
                InstantiateItemCarousel(EItemTypes.Consumables);
                break;
            case ECreatureInteraction.ChangeTrinket:
                InstantiateItemCarousel(EItemTypes.Trinkets);
                break;
            default:
                break;
        }
        Reset();
    }

    void InstantiateItemCarousel(EItemTypes eItemTypes)
    {
        List<InventoryItemData> itemsData = new List<InventoryItemData>();
        itemsData.AddRange(InventoryManager.GetInstance().GetInventoryItemsDataForType(eItemTypes));
        if (itemsData.Count != 0)
        {
            GameMaster.GetInstance().m_carousel.AssignElements(itemsData);
        }
    }
}

namespace Turquoise
{
    public enum ECreatureInteraction
    {
        Swap,
        Infos,
        ChangeTrinket,
        Give,
        Use,
        Count
    }
}