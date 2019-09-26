using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBattle : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_playerCreatures;
    [SerializeField]
    protected List<Creature> m_enemyCreatures;
    [SerializeField]
    protected List<PreBattleCreaturePanel> m_playerPanels;
    [SerializeField]
    protected List<PreBattleCreaturePanel> m_enemyPanels;
    public GameObject m_carouselPrefab;

    public void LaunchPreBattle()
    {
        m_playerCreatures = Player.GetPlayerInstance().GetCreatures();
        AssignCreatures();
    }

    void AssignCreatures()
    {
        for (int i = 0; i < 3; i++)
        {
            if (m_playerCreatures.Count > i)
            {
                m_playerPanels[i].AssignCreature(m_playerCreatures[i].GetData());
            }
            else
            {
                m_playerPanels[i].AssignCreature(null);
            }
        }
    }

    private void Start()
    {
        LaunchPreBattle();
    }
}
