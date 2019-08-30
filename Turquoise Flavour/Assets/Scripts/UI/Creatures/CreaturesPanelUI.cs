using System.Collections;
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

    public void OpenMenu(List<Creature> creatures)
    {
        foreach (var creaturePanel in m_creaturePanels)
        {
            creaturePanel.Reset();
        }
        for (int i = 0; i < creatures.Count && i < 3; i++)
        {
            m_creaturePanels[i].OnOpenMenu(creatures[i]);
        }
    }

    public void OnSubMenuClicked(Creature creature)
    {
        if (m_actionPanelUIInstance != null)
        {
            Destroy(m_actionPanelUIInstance.gameObject);
        }
        m_actionPanelUIInstance = Instantiate(m_actionPanelUIPrefab, transform).GetComponent<ActionPanelUI>();
        m_actionPanelUIInstance.CreatureMenu();

        m_currentCreature = creature;
    }
}

namespace Turquoise
{
    public enum ECreatureInteraction
    {
        Switch,
        Infos,
        ChangeTrinket,
        GiveItem,
        Count
    }
}