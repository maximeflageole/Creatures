using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturesPanelUI : MonoBehaviour
{
    [SerializeField]
    protected List<CreaturePanelUI> m_creaturePanels;
    [SerializeField]
    protected GameObject m_creaturePanelPrefab;


    public void OpenMenu(List<Creature> creatures)
    {
            foreach (var creaturePanel in m_creaturePanels)
            {
                Destroy(creaturePanel.gameObject);
            }
        m_creaturePanels.Clear();
        foreach (var creature in creatures)
        {
            CreaturePanelUI creatureUI = Instantiate(m_creaturePanelPrefab, transform).GetComponent<CreaturePanelUI>();
            creatureUI.OnOpenMenu(creature);
            m_creaturePanels.Add(creatureUI);

        }
    }
}
