using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;

public class GameMaster : MonoBehaviour
{
    static GameMaster s_gmInstance;
    [SerializeField]
    protected List<SEventType> m_eventTypes;
    protected Dictionary<EEventType, GameObject> m_eventTypeDictionary = new Dictionary<EEventType, GameObject>();
    [SerializeField]
    protected TurquoiseEvent m_currentEvent;


    public static GameMaster GetInstance()
    {
        if (s_gmInstance == null)
        {
            s_gmInstance = Instantiate(new GameMaster());
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
                break;
            default:
                break;
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }
}

[System.Serializable]
public struct SEventType
{
    public EEventType eventType;
    public GameObject eventPrefab;
}