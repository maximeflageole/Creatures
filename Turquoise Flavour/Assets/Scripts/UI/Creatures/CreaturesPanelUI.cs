﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using TMPro;

public class CreaturesPanelUI : MonoBehaviour
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

    public void OpenMenu(List<Creature> creatures)
    {
        gameObject.SetActive(true);
        foreach (var creaturePanel in m_creaturePanels)
        {
            creaturePanel.Reset();
        }
        for (int i = 0; i < creatures.Count && i < 3; i++)
        {
            m_creaturePanels[i].OnOpenMenu(creatures[i]);
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
            creature.EquipItem(m_item);
            CloseMenu();
            m_item = EItem.Count;
            return;
        }
        if (m_actionPanelUIInstance != null)
        {
            Destroy(m_actionPanelUIInstance.gameObject);
        }
        if (creature != null)
        {
            m_actionPanelUIInstance = Instantiate(m_actionPanelUIPrefab, transform).GetComponent<ActionPanelUI>();
            m_actionPanelUIInstance.CreatureMenu();
        }

        m_currentCreature = creature;
    }

    public void CloseMenu()
    {
        m_inCreatureSelection = false;
        gameObject.SetActive(false);
    }
}

namespace Turquoise
{
    public enum ECreatureInteraction
    {
        Switch,
        Infos,
        ChangeTrinket,
        Give,
        Use,
        Count
    }
}