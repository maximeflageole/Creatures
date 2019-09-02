using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    protected Color m_unselectedItemColor;
    [SerializeField]
    protected Color m_selectedItemColor;
    [SerializeField]
    protected RectTransform m_inventoryTransform;
    [SerializeField]
    protected GameObject m_inventoryItemPrefab;
    [SerializeField]
    protected List<TupleItemInventory> m_currentItems = new List<TupleItemInventory>();
    [SerializeField]
    protected List<InventoryItemUI> m_itemUis = new List<InventoryItemUI>();
    [SerializeField]
    protected GameObject m_actionPanelUIPrefab;
    protected ActionPanelUI m_actionPanelUIInstance;

    public void SelectTms()
    {
        m_currentItems = InventoryManager.GetInstance().GetInventoryItemsOfType(EItemTypes.TMs);
        UpdateUI();
    }

    public void SelectConsumables()
    {
        m_currentItems = InventoryManager.GetInstance().GetInventoryItemsOfType(EItemTypes.Consumables);
        UpdateUI();
    }

    public void SelectTrinkets()
    {
        m_currentItems = InventoryManager.GetInstance().GetInventoryItemsOfType(EItemTypes.Trinkets);
        UpdateUI();
    }

    public void SelectNeutral()
    {
        m_currentItems = InventoryManager.GetInstance().GetInventoryItemsOfType(EItemTypes.Neutral);
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (var itemUi in m_itemUis)
        {
            Destroy(itemUi.gameObject);
        }
        m_itemUis.Clear();
        foreach (var item in m_currentItems)
        {
            InventoryItemUI uiElement = Instantiate(m_inventoryItemPrefab, m_inventoryTransform).GetComponent<InventoryItemUI>();
            uiElement.SetUI(InventoryManager.GetInstance().GetItemFromEnum(item.itemEnum), item.itemQuantity);
            m_itemUis.Add(uiElement);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectTms();
    }

    public void OnClickItem(InventoryItemData itemData)
    {
        if (m_actionPanelUIInstance != null)
        {
            Destroy(m_actionPanelUIInstance.gameObject);
        }
        m_actionPanelUIInstance = Instantiate(m_actionPanelUIPrefab, transform).GetComponent<ActionPanelUI>() ;
        m_actionPanelUIInstance.CreateMenu(itemData.type);
    }
}