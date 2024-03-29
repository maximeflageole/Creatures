﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ExploratorManager : MonoBehaviour
{
    [SerializeField]
    protected List<sExploratorsTuplet> exploratorsTuplets = new List<sExploratorsTuplet>();
    public List<EExplorator> m_unlockedExplorators = new List<EExplorator>();

    public ExploratorData GetExploratorDataFromExploName(Turquoise.EExplorator explorator)
    {
        foreach (var tuplet in exploratorsTuplets)
        {
            if (tuplet.exploratorName == explorator)
            {
                return tuplet.data;
            }
        }
        return null;
    }

    public static ExploratorManager GetInstance()
    {
        return GameMaster.GetInstance().m_exploratorManager;
    }

    public bool HasUnlockedExplorator(EExplorator explorator)
    {
        return m_unlockedExplorators.Contains(explorator);
    }

    public void LoadGame()
    {
        SaveData saveData = SaveSystem.LoadGame();
        m_unlockedExplorators = saveData.unlockedExplorators;
    }
}

[System.Serializable]
public struct sExploratorsTuplet
{
    public ExploratorData data;
    public Turquoise.EExplorator exploratorName;
}