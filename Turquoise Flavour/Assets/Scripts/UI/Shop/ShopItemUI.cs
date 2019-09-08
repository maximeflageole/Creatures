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
    public bool m_soldOut;
    public bool m_isCardRemove;
    public bool m_isCardUpgrade;

    public InventoryItemData GetShopItem() { return m_shopItem; }

    public void AssignItemAndPrice(InventoryItemData item)
    {
        m_priceTextMesh = GetComponentInChildren<TextMeshProUGUI>();
        m_shopItem = item;
        m_priceTextMesh.text = m_shopItem.cost.ToString();
    }

    public void InstantiateRemovalPanel(int price)
    {
        m_priceTextMesh = GetComponentInChildren<TextMeshProUGUI>();
        m_priceTextMesh.text = price.ToString();
    }

    public void OnClick()
    {
        GetComponentInParent<ShopUI>().OnClick(this, m_isCardRemove, m_isCardUpgrade);
    }

    public void SetAvailability(bool available)
    {
        if (available)
        {
            m_soldOutPanel.SetActive(false);
            m_shopPanel.SetActive(true);
            m_soldOut = false;
        }
        else
        {
            m_soldOutPanel.SetActive(true);
            m_shopPanel.SetActive(false);
            m_soldOut = true;
        }
    }
}
