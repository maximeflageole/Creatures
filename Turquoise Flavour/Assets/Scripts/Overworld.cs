using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;
using Turquoise;
using TMPro;
using UnityEngine.UI;

public class Overworld : MonoBehaviour
{
    public static Overworld s_OverworldInstance;
    public List<ExplorationNodeData> m_nodesData;
    [SerializeField]
    protected List<SEventType> m_eventTypes;
    protected Dictionary<EEventType, GameObject> m_eventTypeDictionary = new Dictionary<EEventType, GameObject>();
    [SerializeField]
    protected GameObject MapPrefab;
    protected ExplorationScreen m_explorationScreen;
    public ExplorationScreen GetExplorationScreen() { return m_explorationScreen; }

    public static Overworld GetInstance()
    {
        if (s_OverworldInstance == null)
        {
            new GameObject("Overworld");
            var go = Instantiate(Resources.Load("Overworld")) as GameObject;
            return go.GetComponent<Overworld>();
        }
        return s_OverworldInstance;
    }

    public static bool HasInstance()
    {
        return s_OverworldInstance != null;
    }

    public GameObject GetObjectFromNode(ExplorationNode explorationNode)
    {
        EEventType eventType = explorationNode.GetEventType();
        return m_eventTypeDictionary[eventType];
    }

    public GameObject GetObjectFromEventType(EEventType eventType)
    {
        return m_eventTypeDictionary[eventType];
    }

    public void StartExploration(List<int> completedNodes)
    {
        if (MapPrefab != null)
        {
            m_explorationScreen = Instantiate(MapPrefab, transform).GetComponent<ExplorationScreen>();
            m_explorationScreen.Init();
            EndInit(completedNodes);
            Player.GetPlayerInstance().EnterOverworld(true);
            GameMaster.GetInstance().ChangeExplorator(Player.GetPlayerInstance().GetCurrentExploratorEnum());
        }
    }

    public void ClickExploratorButton()
    {

    }

    public void EndInit(List<int> completedNodes)
    {
        var explorationNodes = new List<ExplorationNode>();
        foreach (var element in FindObjectsOfType<ExplorationNode>())
        {
            explorationNodes.Add(element);
        }
        //TODO: Node completion is fucked because it is now instantiated differently
        for (int i = 0; i < explorationNodes.Count; i++)
        {
            if (completedNodes.Contains(explorationNodes[i].m_nodeId))
            {
                explorationNodes[i].CompleteNode();
            }
        }
    }

    public static ExplorationNodeData GetExplorationNodeDataFromEventType(EEventType eventType)
    {
        if (s_OverworldInstance == null)
        {
            return null;
        }
        foreach (var nodeData in s_OverworldInstance.m_nodesData)
        {
            if (nodeData.eventType == eventType)
            {
                return nodeData;
            }
        }
        return null;
    }

    public void Awake()
    {
        if (s_OverworldInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_OverworldInstance = this;
        foreach (var eventPair in m_eventTypes)
        {
            m_eventTypeDictionary.Add(eventPair.eventType, eventPair.eventPrefab);
        }
    }

    public MapData GetCurrentMapData()
    {
        return GameMaster.GetInstance().GetMapData();
    }
}

[System.Serializable]
public struct SEventType
{
    public EEventType eventType;
    public GameObject eventPrefab;
}
