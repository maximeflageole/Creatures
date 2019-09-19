using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;
using UnityEngine.SceneManagement;
using UnityEditor;
using Turquoise;

public class GameMaster : MonoBehaviour
{
    protected bool m_inBattle;
    static GameMaster s_gmInstance;
    [SerializeField]
    protected TurquoiseEvent m_currentEvent;
    [SerializeField]
    protected GameObject m_rewardEvent;
    [SerializeField]
    protected ItemRewardEvent m_itemRewardEvent;
    public CardList m_cardList;
    [SerializeField]
    protected GameObject m_cardListPrefab;
    public CreatureList m_creatureList;
    [SerializeField]
    protected GameObject m_creatureListPrefab;
    public LevelUpList m_levelUpList;
    [SerializeField]
    protected GameObject m_levelUpListPrefab;
    public BoonsList m_boonList;
    [SerializeField]
    protected GameObject m_boonListPrefab;
    public CardPileUI m_cardPileUI;
    [SerializeField]
    protected GameObject m_cardPileUIPrefab;
    public AIManager m_aiManager;
    [SerializeField]
    protected GameObject m_aiManagerPrefab;
    public InventoryManager m_inventoryManager;
    [SerializeField]
    protected GameObject m_inventoryManagerPrefab;
    public ExploratorManager m_exploratorManager;
    [SerializeField]
    protected GameObject m_exploratorManagerPrefab;
    [SerializeField]
    protected Canvas m_mainCanvas;
    public GameObject m_exploratorPrefab;
    public List<int> m_completedNodes = new List<int>();
    [SerializeField]
    protected int m_currentNodeIndex = -1;
    public InventoryUI m_inventoryUI;
    public CreaturesPanelUI m_creatureUI;
    public ItemRewardEvent m_itemRewardEventUI;
    public ShopUI m_shopEventUI;
    public GameObject m_endGameText;
    public RewardPanel m_rewardPanel;
    public StatsPanelUI m_statsPanel;
    public CardsPanelUI m_cardsPanelUI;

    [SerializeField]
    protected MapData m_mapData;
    public MapData GetMapData() { return m_mapData; }

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

    public bool GetInBattle()
    {
        return m_inBattle;
    }

    public void SetInBattle(bool inBattle)
    {
        m_inBattle = inBattle;
    }

    void Update()
    {
        //TODO: This should be in overworld only
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
                m_creatureUI.gameObject.SetActive(!m_creatureUI.gameObject.activeSelf);
                if (m_creatureUI.gameObject.activeSelf)
                {
                    m_creatureUI.OpenMenu(Player.GetPlayerInstance().GetCreatures());
                }
                else
                {
                    m_creatureUI.CloseMenu();
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (m_statsPanel != null)
            {
                m_statsPanel.DisplayStats();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCardRemoval();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            List<EExplorator> expList = new List<EExplorator>();
            expList.Add(EExplorator.Captain);
            TheUnlocker.GetInstance().UnlockExplorators(expList);
        }
    }

    public void StartCardRemoval(int amount = 1)
    {
        m_cardPileUI.DisplayCardPile(Player.GetPlayerInstance().GetCurrentCreatureDeck(), false, amount, ECardPickOptions.Remove);
        m_cardPileUI.gameObject.SetActive(true);
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
        if (m_levelUpListPrefab != null)
        {
            m_levelUpList = Instantiate(m_levelUpListPrefab, transform).GetComponent<LevelUpList>();
        }
        if (m_boonListPrefab != null)
        {
            m_boonList = Instantiate(m_boonListPrefab, transform).GetComponent<BoonsList>();
        }
        if (m_cardPileUIPrefab != null)
        {
            if (m_mainCanvas == null)
            {
                m_mainCanvas = FindObjectOfType<Canvas>();
            }
            m_cardPileUI = Instantiate(m_cardPileUIPrefab, m_mainCanvas.transform).GetComponent<CardPileUI>();
        }
        if (m_aiManagerPrefab != null)
        {
            m_aiManager = Instantiate(m_aiManagerPrefab, transform).GetComponent<AIManager>();
        }
        if (m_inventoryManagerPrefab != null)
        {
            m_inventoryManager = Instantiate(m_inventoryManagerPrefab, transform).GetComponent<InventoryManager>();
        }
        if (m_exploratorManagerPrefab != null)
        {
            m_exploratorManager = Instantiate(m_exploratorManagerPrefab, transform).GetComponent<ExploratorManager>();
        }
        Player.GetPlayerInstance().LoadGame();
        m_exploratorManager.LoadGame();
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

    public void StartEvent(ExplorationNode explorationNode, RaycastHit2D hit2D)
    {
        SaveGame();
        var go = Overworld.GetInstance().GetObjectFromNode(explorationNode);
        if(go != null)
        {
            Instantiate(go);
        }

        m_currentNodeIndex = explorationNode.m_nodeId;

        switch (explorationNode.GetEventType())
        {
            case EEventType.Boss:
                hit2D.collider.enabled = false;
                SceneManager.LoadScene("Demo", LoadSceneMode.Single);
                break;
            case EEventType.CardReward:
                hit2D.collider.enabled = false;
                break;
            case EEventType.ItemReward:
                hit2D.collider.enabled = false;
                m_itemRewardEvent.BeginReward();
                break;
            case EEventType.WildEncounter:
                hit2D.collider.enabled = false;
                SceneManager.LoadScene("Demo", LoadSceneMode.Single);
                break;
            case EEventType.Shop:
                m_shopEventUI.ToggleShop();
                break;
            case EEventType.Ship:
                //TODO: The ship is where you can manage your explorators, where you can travel from island to island and is the hub. Lots of work to do here
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

    public int GetCurrentNodeIndex()
    {
        return m_currentNodeIndex;
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
        m_creatureUI.OpenMenu(Player.GetPlayerInstance().GetCreatures());
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
        if (m_inventoryManager != null)
        {
            m_inventoryManager.SetInventoryItems(saveData.inventoryItems);
        }
    }

    public void CompleteNode(int nodeIndex)
    {
        if (nodeIndex != -1)
        {
            if (!m_completedNodes.Contains(nodeIndex))
            {
                m_completedNodes.Add(nodeIndex);

                StatisticsManager.GetInstance().IncrementStat(EStat.NodesCompletedThisRun);
                StatisticsManager.GetInstance().IncrementStat(EStat.NodesCompletedTotal);
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
            gameObject.AddComponent(typeof(BattleStateMachine));
            Player.GetPlayerInstance().EnterOverworld(false);
        }
    }

    public ECard GetRandomUnlockedCard()
    {
        //TODO: Unlocked cards, not all cards. Also, add a rarity parameter
        ECard card = ECard.Count;
        if (m_cardList != null)
        {
            card = m_cardList.GetRandomCard();
        }
        return card;
    }

    public void OpenMenuToGiveItem(EItem item)
    {
        m_inventoryUI.Close();
        m_creatureUI.GetComponent<CreaturesPanelUI>().StartSelectCreatureForItem(item);
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