using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticEnhancementPanel : MonoBehaviour
{
    [SerializeField]
    protected List<GeneticEnhancementLevel> m_levelPanels;

    [SerializeField]
    protected Creature m_creature;
    protected CreatureEvolutionTree m_evolutionTree;

    public void OnInstantiate(Creature creature)
    {
        m_creature = creature;
        m_evolutionTree = m_creature.GetData().evolutionTree;
        int i = 0;
        foreach (var panel in m_levelPanels)
        {
            if (panel != null)
            {
                panel.OnInstantiate(m_evolutionTree.evolutionNodes[i]);
            }
            i++;
        }
    }

    void Start()
    {
        OnInstantiate(Player.GetPlayerInstance().GetCurrentCreature());
    }

    public void OnSelectEvolution(int level, bool left)
    {

    }
}
