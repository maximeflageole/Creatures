using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_priceTextMesh;
    [SerializeField]
    protected GameObject m_shopPanel;
    [SerializeField]
    protected GameObject m_soldOutPanel;
    [SerializeField]
    protected InventoryItemData m_shopItem;
    public InventoryItemData GetShopItem() { return m_shopItem; }

    public void Start()
    {
        m_priceTextMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AssignItemAndPrice(InventoryItemData item)
    {
        m_shopItem = item;
        m_priceTextMesh.text = m_shopItem.cost.ToString();
    }

    public void OnClick()
    {
        GetComponentInParent<ShopUI>().OnClick(this);
    }

    public void SetAvailability(bool available)
    {
        if (available)
        {
            m_soldOutPanel.SetActive(false);
            m_shopPanel.SetActive(true);
        }
        else
        {
            m_soldOutPanel.SetActive(true);
            m_shopPanel.SetActive(false);
        }
    }
}
