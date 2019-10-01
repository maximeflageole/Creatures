using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBattleRewardUI : MonoBehaviour
{
    public delegate void CallbackType();

    public Transform m_contentParent;
    public List<InventoryItemUI> m_gainedItems;
    [SerializeField]
    protected GameObject m_itemUIPrefab;
    protected CallbackType m_rewardsPickedCallback; // to store the function

    public void InstantiatePostBattleReward(CallbackType callback)
    {
        var itemsData = TheRewarder.sInstance.GetBattleRewards();
        int goldAmount = TheRewarder.sInstance.GetGoldAmount();
        var goldItemData = InventoryManager.GetInstance().GetGoldItemData();
        m_rewardsPickedCallback = callback;
        Reset();
        gameObject.SetActive(true);
        Debug.Log("POST BATTLE REWARD SUPPOSED TO WORK");
        foreach (var itemData in itemsData)
        {
            var itemUI = Instantiate(m_itemUIPrefab, m_contentParent).GetComponent<InventoryItemUI>();
            itemUI.SetUI(itemData);
            m_gainedItems.Add(itemUI);
        }
        var goldUI = Instantiate(m_itemUIPrefab, m_contentParent).GetComponent<InventoryItemUI>();
        goldUI.SetUI(goldItemData, goldAmount);
        m_gainedItems.Add(goldUI);
        InventoryManager.GetInstance().AddInventoryItem(goldItemData, goldAmount);
    }

    public void NextButtonPressed()
    {
        foreach (var item in m_gainedItems)
        {
            InventoryManager.GetInstance().AddInventoryItem(item.GetItemData());
        }
        m_rewardsPickedCallback();
        Reset();
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        foreach (var item in m_gainedItems)
        {
            Destroy(item.gameObject);
        }
    }
}
