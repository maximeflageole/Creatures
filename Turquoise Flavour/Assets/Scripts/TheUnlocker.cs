using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class TheUnlocker : MonoBehaviour
{
    public static TheUnlocker m_instance;
    public static TheUnlocker GetInstance() { return m_instance; }
    [SerializeField]
    protected List<UnlockData> m_unlocksData;

    List<EExplorator> m_unlockedExploratorsAtGameEnds = new List<EExplorator>();

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    public void UnlockExplorators(List<EExplorator> explorators, bool rightAway = true)
    {
        if (rightAway)
        {
            List<Sprite> sprites = new List<Sprite>();
            List<string> str = new List<string>();

            var exploManager = ExploratorManager.GetInstance();
            foreach (var explo in explorators)
            {
                if (!exploManager.m_unlockedExplorators.Contains(explo))
                {
                    ExploratorData exploratorData = exploManager.GetExploratorDataFromExploName(explo);
                    sprites.Add(exploratorData.sprite);
                    str.Add(exploratorData.explorator.ToString());
                    exploManager.m_unlockedExplorators.Add(explo);
                }
            }
            if (str.Count > 0)
            {
                GameMaster.GetInstance().m_rewardPanel.UnlockReward(str, sprites);
                SaveSystem.SaveGame();
            }
        }
        else
        {
            m_unlockedExploratorsAtGameEnds.AddRange(explorators);
        }
    }

    public void OnGamesEnd()
    {
        UnlockExplorators(m_unlockedExploratorsAtGameEnds);
    }

    public void Update()
    {
        //TODO: Probably not put this in a for loop dummy
        foreach (var data in m_unlocksData)
        {
            bool unlocked = false;
            int statNumber = StatisticsManager.GetInstance().GetStatAmount(data.stat);
            if (data.comparisonStat != EStat.Count)
            {
                unlocked = ValidateCondition(data.operation, statNumber, StatisticsManager.GetInstance().GetStatAmount(data.comparisonStat));
            }
            else
            {
                unlocked = ValidateCondition(data.operation, statNumber, data.comparisonNumber);
            }
            if (unlocked)
            {
                UnlockContent(data);
            }
        }
    }

    public bool ValidateCondition(EOperation operation, int a, int b)
    {
        switch (operation)
        {
            case EOperation.Bigger:
                return (a > b);
                
            case EOperation.BiggerOrEqual:
                return (a >= b);
                
            case EOperation.Equals:
                return (a == b);
                
            case EOperation.Less:
                return (a < b);
                
            case EOperation.LessOrEqual:
                return (a <= b);
                
            case EOperation.NotEqual:
                return (a != b);
        }
        return false;
    }

    void UnlockContent(UnlockData data)
    {
        UnlockExplorators(data.exploratorsUnlocks);
        foreach (var unlock in data.cardsUnlocks)
        {
            //TODO
        }
        foreach (var unlock in data.itemsUnlocks)
        {
            //TODO
        }
    }
}
