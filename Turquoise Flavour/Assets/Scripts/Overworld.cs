using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;
using Turquoise;
using TMPro;

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
    public InventoryUI m_inventoryUI;
    public GameObject m_creatureUI;
    public ItemRewardEvent m_itemRewardEventUI;
    [SerializeField]
    protected TextMeshProUGUI m_goldTextMesh;

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

    public void OpenMenuToGiveItem(EItem item)
    {
        m_inventoryUI.Close();
        m_creatureUI.GetComponent<CreaturesPanelUI>().StartSelectCreatureForItem(item);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_inventoryUI != null)
            {
                m_inventoryUI.Toggle();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_creatureUI != null)
            {
                m_creatureUI.SetActive(!m_creatureUI.activeSelf);
                if (m_creatureUI.activeSelf)
                {
                    m_creatureUI.GetComponent<CreaturesPanelUI>().OpenMenu(Player.GetPlayerInstance().GetCreatures());
                }
                else
                {
                    m_creatureUI.GetComponent<CreaturesPanelUI>().CloseMenu();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (m_itemRewardEventUI != null)
            {
                m_itemRewardEventUI.BeginReward();
            }
        }
        m_goldTextMesh.text = InventoryManager.GetInstance().GetPlayerGold().ToString();
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
        }
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
        return m_explorationScreen.GetMapData();
    }
}

[System.Serializable]
public struct SEventType
{
    public EEventType eventType;
    public GameObject eventPrefab;
}
