using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager s_instance;
    public static StatisticsManager GetInstance() { return s_instance; }

    [SerializeField]
    protected List<SStatTuple> m_statistics = new List<SStatTuple>();
    public List<SStatTuple> GetStatistics() { return m_statistics; }

    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }
    }

    private void Start()
    {
        LoadStats();
    }

    public void LoadStats()
    {
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
        {
            print("StatisticsManager::Save file empty");
            return;
        }
        m_statistics = saveData.gameStatistics;
    }

    public void IncrementStat(EStat stat, int amount = 1)
    {
        for (int i = 0; i < m_statistics.Count; i++)
        {
            if (m_statistics[i].stat == stat)
            {
                m_statistics[i] = new SStatTuple { stat = stat, value = m_statistics[i].value + amount };
                return;
            }
        }
        m_statistics.Add(new SStatTuple { stat = stat, value = amount });
    }
    
    public int GetStatAmount(EStat stat)
    {
        foreach (var tuple in m_statistics)
        {
            if (tuple.stat == stat)
            {
                return tuple.value;
            }
        }
        return 0;
    }
}

namespace Turquoise
{
    [System.Serializable]
    public enum EStat
    {
        NodesCompletedTotal,
        NodesCompletedThisRun,
        CreaturesCapturedTotal, //TODO
        CreaturesCapturedThisRun, //TODO
        DamageInflictedTotal,
        DamageInflictedThisRun,
        DamageReceivedTotal,
        DamageReceivedThisRun,
        HealingTotal,
        HealingThisRun,
        Count
    }

    [System.Serializable]
    public struct SStatTuple
    {
        public EStat stat;
        public int value;
    }
}
