using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;
using UnityEngine.SceneManagement;
using UnityEditor;

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
            return go.GetComponent<GameMaster>();
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
        Player.GetPlayerInstance();

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
        Player.GetPlayerInstance().LoadGame();
    }

    public void EndCurrentEvent(bool success = false)
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        if (success)
        {
            CompleteNode(m_currentNodeIndex);
        }
        SaveGame();
    }

    public void StartEvent(ExplorationNode explorationNode)
    {
        if (SaveGame())
        {
            EEventType eventType = explorationNode.m_eventType;
            if (m_eventTypeDictionary[eventType] != null)
            {
                Instantiate(m_eventTypeDictionary[eventType]);
            }

            m_currentNodeIndex = explorationNode.m_nodeId;

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
    }

    public bool SaveGame()
    {
        return SaveSystem.SaveGame();
    }
    
    public static void ResetSave()
    {
        SaveSystem.ResetSave();
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

    //TODO: this needs to be revised asap
    public void CompleteNode(ExplorationNode explorationNode)
    {
        if (!m_completedNodes.Contains(explorationNode.m_nodeId))
        {
            m_completedNodes.Add(explorationNode.m_nodeId);
        }
        explorationNode.CompleteNode();
    }

    public void CompleteNode(int nodeIndex)
    {
        if (!m_completedNodes.Contains(nodeIndex))
        {
            foreach (var explorationNode in m_explorationNodes)
            {
                if (explorationNode.m_nodeId == nodeIndex)
                {
                    m_completedNodes.Add(nodeIndex);
                    explorationNode.CompleteNode();
                    break;
                }
            }
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            LoadGame();
            if (GetComponent<ExplorationScreen>() == null)
            {
                gameObject.AddComponent<ExplorationScreen>();
            }
        }
        else
        {
            var explorationScreen = gameObject.GetComponent<ExplorationScreen>();
            if (explorationScreen != null)
            {
                Destroy(explorationScreen);
            }
        }
        if (scene.name == "Demo")
        {
            CardEffects.GetCardEffectsInstance().Initialization();
        }
    }
}

[CustomEditor(typeof(GameMaster))]

public class GameMasterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.Button(new Rect(20, 800, 100, 30), new GUIContent("Reset Save")))
            GameMaster.ResetSave();
    }
}


[System.Serializable]
public struct SEventType
{
    public EEventType eventType;
    public GameObject eventPrefab;
}