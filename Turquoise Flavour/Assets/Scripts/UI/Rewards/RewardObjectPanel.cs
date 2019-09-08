using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardObjectPanel : MonoBehaviour
{
    GameObject m_child;

    public void DestroyChild()
    {
        if (m_child != null)
        {
            Destroy(m_child);
        }
    }

    public void AssignChild(GameObject go)
    {
        DestroyChild();
        m_child = go;
    }

    public void AssignData(InventoryItemData itemData, int qty)
    {
        var itemUI = GetComponentInChildren<InventoryItemUI>();
        if (itemUI != null)
        {
            itemUI.SetUI(itemData, qty);
        }
    }
}
