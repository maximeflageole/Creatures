using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public string description;
    public int cost;
    public Turquoise.ERarity rarity;
    public Turquoise.EItemTypes type;
    public Turquoise.EItem item;
}
