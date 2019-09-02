using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_textMeshProUGUI;
    [SerializeField]
    protected Image m_image;
    [SerializeField]
    protected InventoryItemData m_itemData;

    public void SetUI(InventoryItemData itemData, int qty)
    {
        m_itemData = itemData;
        m_image.sprite = itemData.sprite;
        if (qty > 1)
        {
            m_textMeshProUGUI.text = itemData.name + " * " + qty;
        }
        else
        {
            m_textMeshProUGUI.text = itemData.name;
        }
    }

    public void OnCLick()
    {
        GetComponentInParent<InventoryUI>().OnClickItem(m_itemData);
    }
}
