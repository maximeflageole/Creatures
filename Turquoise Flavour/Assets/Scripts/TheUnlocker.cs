using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class TheUnlocker : MonoBehaviour
{
    public static TheUnlocker m_instance;
    public static TheUnlocker GetInstance() { return m_instance; }

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
            }
            SaveSystem.SaveGame();
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
}
