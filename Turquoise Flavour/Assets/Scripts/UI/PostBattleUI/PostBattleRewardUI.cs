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

    public void InstantiatePostBattleReward(List<InventoryItemData> itemsData, CallbackType callback)
    {
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
