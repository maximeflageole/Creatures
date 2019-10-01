using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class TheRewarder : MonoBehaviour
{
    [SerializeField]
    protected List<InventoryItemData> m_battleRewards;
    public static TheRewarder sInstance;

    public void Awake()
    {
        if (sInstance != null)
        {
            if (sInstance != this)
            {
                Destroy(this);
                return;
            }
        }
        sInstance = this;
    }

    public void GenerateRewards(EventType eventType, int islandLevel, List<InventoryItemData> extraBattleRewards = null)
    {
    }

    public List<InventoryItemData> GetBattleRewards()
    {
        return m_battleRewards;
    }

    public int GetGoldAmount()
    {
        return Random.Range(10, 20);
    }
}
