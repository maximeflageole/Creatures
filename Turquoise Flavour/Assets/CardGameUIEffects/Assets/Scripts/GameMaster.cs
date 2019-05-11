using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    static GameMaster s_gmInstance;
    [SerializeField]
    protected List<SEventType> m_eventTypes;
    protected Dictionary<EEventType, GameObject> m_eventTypeDictionary = new Dictionary<EEventType, GameObject>();
    [SerializeField]
    protected TurquoiseEvent m_currentEvent;
    public CardList m_cardList;
    [SerializeField]
    protected GameObject m_cardListPrefab;
    public CreatureList m_creatureList;
    [SerializeField]
    protected GameObject m_creatureListPrefab;
    public List<int> m_completedNodes = new List<int>();
    [SerializeField]
    protected List<ExplorationNode> m_explorationNodes = new List<ExplorationNode>();
    [SerializeField]
    protected int m_currentNodeIndex;


    public static GameMaster GetInstance()
    {
        if (s_gmInstance == null)
        {
            var gameMaster = new GameObject("_GM");
            var go = Instantiate(Resources.Load("_GM")) as GameObject;
            s_gmInstance = go.GetComponent<GameMaster>();
        }
        return s_gmInstance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (s_gmInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_gmInstance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);

        foreach (var eventPair in m_eventTypes)
        {
            m_eventTypeDictionary.Add(eventPair.eventType, eventPair.eventPrefab);
        }
        if (m_creatureListPrefab != null)
        {
            m_creatureList = Instantiate(m_creatureListPrefab, transform).GetComponent<CreatureList>();
        }
        if (m_cardListPrefab != null)
        {
            m_cardList = Instantiate(m_cardListPrefab, transform).GetComponent<CardList>();
        }
    }

    public void ChangeEvent(ExplorationNode explorationNode)
    {
        EEventType eventType = explorationNode.m_eventType;
        if (m_eventTypeDictionary[eventType] != null)
        {
            Instantiate(m_eventTypeDictionary[eventType]);
        }

        m_currentNodeIndex = explorationNode.m_nodeId;
        CompleteNode(explorationNode);

        switch (eventType)
        {
            case EEventType.Boss:
                break;
            case EEventType.CardReward:
                break;
            case EEventType.Treasure:
                break;
            case EEventType.WildEncounter:
                SceneManager.LoadScene("Demo", LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void ChangeCreature()
    {
        Player.GetPlayerInstance().SwapCreature();
    }

    public void LoadGame()
    {
        m_explorationNodes.Clear();
        foreach (var element in FindObjectsOfType<ExplorationNode>())
        {
            m_explorationNodes.Add(element);
        }
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
        {
            print("Save file empty");
            return;
        }
        m_completedNodes.Clear();
        m_completedNodes = saveData.completedNodes;
        for (int i = 0; i < m_explorationNodes.Count; i++)
        {
            if (m_completedNodes.Contains(m_explorationNodes[i].m_nodeId))
            {
                CompleteNode(m_explorationNodes[i]);
            }
        }
    }

    public void CompleteNode(ExplorationNode explorationNode)
    {
        if (!m_completedNodes.Contains(explorationNode.m_nodeId))
        {
            m_completedNodes.Add(explorationNode.m_nodeId);
        }
        explorationNode.CompleteNode();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            LoadGame();
            CompleteNode(m_explorationNodes[m_currentNodeIndex]);
        }
        if (scene.name == "Demo")
        {
            CardEffects.GetCardEffectsInstance().Initialization();
        }
    }
}

[System.Serializable]
public struct SEventType
{
    public EEventType eventType;
    public GameObject eventPrefab;
}