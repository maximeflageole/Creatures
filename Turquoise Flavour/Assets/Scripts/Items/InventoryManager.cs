﻿using System.Collections;
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

    public void AddInventoryItemFromEItem(EItem item, int amount = 1)
    {
        var itemData = GetItemFromEnum(item);
        AddInventoryItem(itemData, amount);
    }

    [SerializeField]
    protected List<TupleItemInventory> m_inventoryItemsTuples = new List<TupleItemInventory>();

    public void SetInventoryItems(List<TupleItemInventory> inventoryItemTuples)
    {
        m_inventoryItemsTuples = inventoryItemTuples;
    }

    public List<TupleItemInventory> GetInventoryItems()
    {
        return m_inventoryItemsTuples;
    }

    public void AddInventoryItem(InventoryItemData inventoryItemData, int amount = 1)
    {
        int itemToDestroyIndex = -1;
        EItemTypes itemType = inventoryItemData.type;
        bool itemAdded = false;
        for (int i = 0; i< m_inventoryItemsTuples.Count; i++)
        {
            if (GetItemFromEnum(m_inventoryItemsTuples[i].itemEnum).item == inventoryItemData.item)
            {
                m_inventoryItemsTuples[i].itemQuantity += amount;
                itemAdded = true;
                if (m_inventoryItemsTuples[i].itemQuantity <= 0)
                {
                    itemToDestroyIndex = m_inventoryItemsTuples.IndexOf(m_inventoryItemsTuples[i]);
                }
            }
        }
        if (!itemAdded)
        {
            m_inventoryItemsTuples.Add(new TupleItemInventory(inventoryItemData.item, amount));
        }
        if (itemToDestroyIndex != -1)
        {
            m_inventoryItemsTuples.RemoveAt(itemToDestroyIndex);
        }
        SaveSystem.SaveGame();
    }

    public void RemoveInventoryItem(InventoryItemData inventoryItemData, int amount = 1)
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

    public List<TupleItemInventory> GetInventoryItemsOfType(EItemTypes itemType)
    {
        List<TupleItemInventory> returnItems = new List<TupleItemInventory>();
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

    public InventoryItemData GetRandomItem(EItemTypes itemType)
    {
        List<InventoryItemData> itemDatas = new List<InventoryItemData>();
        foreach (var itemData in m_itemDictionnary)
        {
            if (itemData.Value.type == itemType)
            {
                itemDatas.Add(itemData.Value);
            }
        }
        int randomIndex = Random.Range(0, itemDatas.Count);
        return itemDatas[randomIndex];
    }

    public InventoryItemData GetGold()
    {
        return m_itemDictionnary[EItem.Gold];
    }
}

[System.Serializable]
public class TupleItemInventory
{
    public EItem itemEnum;
    public int itemQuantity;
    public TupleItemInventory(EItem i, int q) { itemEnum = i; itemQuantity = q; }
}

namespace Turquoise
{
    public enum EItemTypes
    {
        TMs,
        Consumables,
        Trinkets,
        Neutral,
        Gold,
        Count
    }

    public enum EItem
    {
        HealthPotion,
        DamagePotion,
        HealthTrinket,
        NormalTrinket,
        Gold,
        TestTM,
        TestNeutral,
        Count
    }

    [System.Serializable]
    public struct sItemTuplet
    {
        public EItem item;
        public InventoryItemData itemData;
    }

    public enum EItemInteraction
    {
        Use,
        Give,
        Toss,
        Count
    }
}
