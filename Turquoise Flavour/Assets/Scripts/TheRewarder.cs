using System.Collections.Generic;
using UnityEngine;

public class TheRewarder : MonoBehaviour
{
    public static TheRewarder sInstance;
    int m_goldAmount;
    List<InventoryItemData> m_itemsReward = default;
    [SerializeField]
    protected CreatureRewardData rewardData;

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

    public List<InventoryItemData> GetItemRewards()
    {
        return m_itemsReward;
    }

    public int GetGoldReward()
    {
        return m_goldAmount;
    }

    public void GenerateRewards()
    {
        //TODO: Remove
        GenerateRewards(rewardData.rewards);
    }
    public void GenerateRewards(List<sRewardOdds> rewards)
    {
        m_itemsReward = new List<InventoryItemData>();
        int random0to100 = Random.Range(0, 100);
        foreach (var reward in rewards)
        {
            random0to100 -= reward.oddsOnHundred;
            if (random0to100 <= 0)
            {
                m_goldAmount = (int)Random.Range(reward.m_goldMinMax.x, reward.m_goldMinMax.y);
                foreach (var rarity in reward.itemRarities)
                {
                    m_itemsReward.AddRange(InventoryManager.GetInstance().GetRandomItemsOfRarity(rarity, 1));
                }
                break;
            }
        }
    }
}
