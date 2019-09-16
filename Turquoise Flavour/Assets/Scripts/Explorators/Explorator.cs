using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class Explorator : MonoBehaviour
{
    [SerializeField]
    protected ExplorationNode m_currentNode;
    [SerializeField]
    protected ExploratorData m_exploratorData;

    public void SelectNode(ExplorationNode node)
    {
        m_currentNode = node;
    }

    public bool IsNodeSelected(ExplorationNode node)
    {
        return m_currentNode == node;
    }

    public void ChangeExplorator(EExplorator explorator)
    {
        m_exploratorData = GameMaster.GetInstance().m_exploratorManager.GetExploratorDataFromExploName(explorator);
        if (m_exploratorData != null)
        {
            GetComponent<SpriteRenderer>().sprite = m_exploratorData.sprite;
            SaveSystem.SaveGame();
            Overworld.GetInstance().ChangeExplorator(explorator);
        }
    }

    public EExplorator GetCurrentExplorator()
    {
        return m_exploratorData.explorator;
    }
}
