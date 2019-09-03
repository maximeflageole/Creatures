using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public string description;
    public int cost;
    public ERarity rarity;
    public EItemTypes type;
    public EItem item;
    public List<SAbilityEffect> effects;
}
