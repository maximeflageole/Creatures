using System.Collections;
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
}

[System.Serializable]
public struct sExploratorsTuplet
{
    public ExploratorData data;
    public Turquoise.EExplorator exploratorName;
}