﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using TMPro;

public class ActionPanelUI : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> m_child;
    [SerializeField]
    protected GameObject m_menuInteractionPrefab;

    public void CreateMenu(EItemTypes itemType)
    {
        List<EItemInteraction> itemInteractions = GetItemInteractions(itemType);
        foreach (var interaction in itemInteractions)
        {
            var menuInteraction = Instantiate(m_menuInteractionPrefab, transform);
            m_child.Add(menuInteraction);
            menuInteraction.GetComponentInChildren<TextMeshProUGUI>().text = interaction.ToString();
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50 * m_child.Count);
    }

    public void OnClickItemAction(EItemInteraction action)
    {
        switch (action)
        {
            case EItemInteraction.Use:
                break;
            case EItemInteraction.Give:
                break;
            case EItemInteraction.Toss:
                break;
        }
        Destroy(gameObject);
    }

    public static List<EItemInteraction> GetItemInteractions(EItemTypes itemType)
    {
        List<EItemInteraction> returnInteractions = new List<EItemInteraction>();
        switch (itemType)
        {
            case EItemTypes.Consumables:
                returnInteractions.Add(EItemInteraction.Use);
                returnInteractions.Add(EItemInteraction.Toss);
                break;
            case EItemTypes.TMs:
                returnInteractions.Add(EItemInteraction.Use);
                returnInteractions.Add(EItemInteraction.Toss);
                break;
            case EItemTypes.Trinkets:
                returnInteractions.Add(EItemInteraction.Give);
                returnInteractions.Add(EItemInteraction.Toss);
                break;
            case EItemTypes.Neutral:
                returnInteractions.Add(EItemInteraction.Use);
                returnInteractions.Add(EItemInteraction.Toss);
                break;
            default: break;
        }
        return returnInteractions;
    }

    public void CreatureMenu()
    {
        List<ECreatureInteraction> creatureInteractions = new List<ECreatureInteraction>();
        creatureInteractions.Add(ECreatureInteraction.Infos);
        creatureInteractions.Add(ECreatureInteraction.GiveItem);
        creatureInteractions.Add(ECreatureInteraction.ChangeTrinket);

        foreach (var interaction in creatureInteractions)
        {
            var menuInteraction = Instantiate(m_menuInteractionPrefab, transform);
            m_child.Add(menuInteraction);
            menuInteraction.GetComponentInChildren<TextMeshProUGUI>().text = interaction.ToString();
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50 * m_child.Count);
    }
}
