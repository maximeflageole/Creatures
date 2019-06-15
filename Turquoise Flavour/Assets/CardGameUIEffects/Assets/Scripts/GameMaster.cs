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
    protected TurquoiseEvent m_currentEvent;
    [SerializeField]
    protected GameObject m_rewardEvent;
    public CardList m_cardList;
    [SerializeField]
    protected GameObject m_cardListPrefab;
    public CreatureList m_creatureList;
    [SerializeField]
    protected GameObject m_creatureListPrefab;
    public List<int> m_completedNodes = new List<int>();
    [SerializeField]
    protected int m_currentNodeIndex = -1;

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

    public GameObject GetRewardPrefab()
    {
        return m_rewardEvent;
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
        SaveGame();
        var go = Overworld.GetInstance().GetObjectFromNode(explorationNode);
        if(go != null)
        {
            Instantiate(go);
        }

        m_currentNodeIndex = explorationNode.m_nodeId;

        switch (explorationNode.m_eventType)
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

    public void StartEventOfEventType(EEventType eventType)
    {
        m_currentNodeIndex = -1;
        var go = Overworld.GetInstance().GetObjectFromEventType(eventType);
        if (go != null)
        {
            Instantiate(go);
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
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
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
        {
            print("Save file empty");
            return;
        }
        m_completedNodes.Clear();
        m_completedNodes = saveData.completedNodes;
    }

    public void CompleteNode(int nodeIndex)
    {
        if (nodeIndex != -1)
        {
            if (!m_completedNodes.Contains(nodeIndex))
            {
                m_completedNodes.Add(nodeIndex);
            }
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            LoadGame();
            if (Player.GetPlayerInstance().HasCreatures())
            {
                Overworld.GetInstance().StartExploration(m_completedNodes);
            }
            else
            {
                StartEventOfEventType(EEventType.CreaturePick);
            }
        }
        if (scene.name == "Demo")
        {
            BattleStateMachine battleStateMachine = gameObject.AddComponent(typeof(BattleStateMachine)) as BattleStateMachine;
        }
    }
}

[CustomEditor(typeof(GameMaster))]

public class GameMasterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.Button(new Rect(20, 400, 100, 30), new GUIContent("Reset Save")))
            GameMaster.ResetSave();
    }
}