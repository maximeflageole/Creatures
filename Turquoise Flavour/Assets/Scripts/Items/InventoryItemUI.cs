using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    protected string m_itemName;
    [SerializeField]
    protected TextMeshProUGUI m_textMeshProUGUI;
    [SerializeField]
    protected Image m_image;

    public void SetUI(InventoryItemData itemData, int qty)
    {
        m_image.sprite = itemData.sprite;
        m_textMeshProUGUI.text = itemData.name + " * " + qty;
    }
}
