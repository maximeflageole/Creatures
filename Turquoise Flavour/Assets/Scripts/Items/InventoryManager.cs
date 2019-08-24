using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    protected List<sItemTuplet> m_itemTuplets = new List<sItemTuplet>();
    public Dictionary<EItem, InventoryItemData> m_itemDictionnary = new Dictionary<EItem, InventoryItemData>();


    public static InventoryManager m_instance;
    public static InventoryManager GetInstance()
    {
        return m_instance;
    }

    [SerializeField]
    protected List<sTupleItemInventory> m_inventoryItemsTuples = new List<sTupleItemInventory>();

    public void SetInventoryItems(List<sTupleItemInventory> inventoryItemTuples)
    {
        m_inventoryItemsTuples = inventoryItemTuples;
    }

    public List<sTupleItemInventory> GetInventoryItems()
    {
        return m_inventoryItemsTuples;
    }

    public void AddInventoryItem(InventoryItemData inventoryItemData, int amount)
    {
        int itemToDestroyIndex = -1;
        EItemTypes itemType = inventoryItemData.type;
        foreach (var item in m_inventoryItemsTuples)
        {
            if (GetItemFromEnum(item.itemEnum).type == itemType)
            {
                item.AddItemQty(amount);
                if (item.itemQuantity == 0)
                {
                    itemToDestroyIndex = m_inventoryItemsTuples.IndexOf(item);
                }
            }
        }
        if (itemToDestroyIndex != -1)
        {
            m_inventoryItemsTuples.RemoveAt(itemToDestroyIndex);
        }
    }

    public void RemoveInventoryItem(InventoryItemData inventoryItemData, int amount)
    {
        AddInventoryItem(inventoryItemData, -amount);
    }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            foreach (var item in m_itemTuplets)
            {
                m_itemDictionnary.Add(item.item, item.itemData);
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public List<sTupleItemInventory> GetInventoryItemsOfType(EItemTypes itemType)
    {
        List<sTupleItemInventory> returnItems = new List<sTupleItemInventory>();
        foreach (var item in m_inventoryItemsTuples)
        {
            if (GetItemFromEnum(item.itemEnum).type == itemType)
            {
                returnItems.Add(item);
            }
        }
        return returnItems;
    }

    public InventoryItemData GetItemFromEnum(EItem item)
    {
        return m_itemDictionnary[item];
    }
}

[System.Serializable]
public struct sTupleItemInventory
{
    public EItem itemEnum;
    public int itemQuantity;
    public void AddItemQty(int qty)
    {
        itemQuantity += qty;
    }
}

namespace Turquoise
{
    public enum EItemTypes
    {
        TMs,
        Consumables,
        Trinkets,
        Neutral,
        Count
    }

    public enum EItem
    {
        HealthPotion,
        DamagePotion,
        HealthTrinket,
        NormalTrinket,
        Count
    }

    [System.Serializable]
    public struct sItemTuplet
    {
        public EItem item;
        public InventoryItemData itemData;
    }
}
