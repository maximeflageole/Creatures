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
    public CardList cardList;
    [SerializeField]
    protected GameObject cardListPrefab;
    public CreatureList creatureList;
    [SerializeField]
    protected GameObject creatureListPrefab;


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
        s_gmInstance = this;
        DontDestroyOnLoad(gameObject);
        foreach (var eventPair in m_eventTypes)
        {
            m_eventTypeDictionary.Add(eventPair.eventType, eventPair.eventPrefab);
        }
        if (creatureListPrefab != null)
        {
            creatureList = Instantiate(creatureListPrefab, transform).GetComponent<CreatureList>();
        }
        if (cardListPrefab != null)
        {
            cardList = Instantiate(cardListPrefab, transform).GetComponent<CardList>();
        }
    }

    public void ChangeEvent(EEventType eventType)
    {
        if (m_eventTypeDictionary[eventType] != null)
        {
            Instantiate(m_eventTypeDictionary[eventType]);
        }

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
}

[System.Serializable]
public struct SEventType
{
    public EEventType eventType;
    public GameObject eventPrefab;
}