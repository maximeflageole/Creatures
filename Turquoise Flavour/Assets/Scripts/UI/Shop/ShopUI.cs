using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Turquoise;

public class ShopUI : MonoBehaviour
{
    protected List<GameObject> m_instances = new List<GameObject>();
    [SerializeField]
    protected GameObject m_itemPrefab;
    [SerializeField]
    protected GameObject m_cardPrefab;
    [SerializeField]
    protected GameObject m_upgradePrefab;
    [SerializeField]
    protected GameObject m_removePrefab;
    [SerializeField]
    protected TextMeshProUGUI m_goldTextMesh;
    [SerializeField]
    protected List<ShopItemUI> m_tmPanels;
    [SerializeField]
    protected List<ShopItemUI> m_consumablePanels;
    [SerializeField]
    protected List<ShopItemUI> m_neutralPanels;
    [SerializeField]
    protected List<ShopItemUI> m_trinketPanels;
    [SerializeField]
    protected ShopItemUI m_upgradePanel;
    [SerializeField]
    protected ShopItemUI m_removePanel;
    protected List<InventoryItemData> m_shopItems = new List<InventoryItemData>();

    public void Start()
    {
        Reset();
    }

    public void Reset()
    {
        m_goldTextMesh.text = InventoryManager.GetInstance().GetPlayerGold().ToString();
        foreach (var obj in m_instances)
        {
            Destroy(obj);
        }
        m_instances.Clear();
        m_shopItems.Clear();
        GenerateContent();
        PopulateShop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    void GenerateContent()
    {
        foreach (var tmPanel in m_tmPanels)
        {
            m_shopItems.Add(InventoryManager.GetInstance().GetRandomItem(EItemTypes.TMs));
        }
        foreach (var consPanel in m_consumablePanels)
        {
            m_shopItems.Add(InventoryManager.GetInstance().GetRandomItem(EItemTypes.Consumables));
        }
        foreach (var neutralPanel in m_neutralPanels)
        {
            m_shopItems.Add(InventoryManager.GetInstance().GetRandomItem(EItemTypes.Neutral));
        }
        foreach (var trinketPanel in m_trinketPanels)
        {
            m_shopItems.Add(InventoryManager.GetInstance().GetRandomItem(EItemTypes.Trinkets));
        }
    }

    public void PopulateShop()
    {
        int i = 0;
        foreach (var tmPanel in m_tmPanels)
        {
            m_instances.Add(Instantiate(m_cardPrefab, tmPanel.transform));
            //TODO: Cards item ui
            //InventoryItemUI itemUI = Instantiate(m_cardPrefab, tmPanel.transform).GetComponent<InventoryItemUI>();
            //itemUI.SetUI(m_shopItems[i]);
            tmPanel.AssignPrice(m_shopItems[i].cost);
            i++;
        }
        foreach (var consPanel in m_consumablePanels)
        {
            InventoryItemUI itemUI = Instantiate(m_itemPrefab, consPanel.transform).GetComponent<InventoryItemUI>();
            m_instances.Add(itemUI.gameObject);
            itemUI.SetUI(m_shopItems[i]);
            consPanel.AssignPrice(m_shopItems[i].cost);
            i++;
        }
        foreach (var neutralPanel in m_neutralPanels)
        {
            InventoryItemUI itemUI = Instantiate(m_itemPrefab, neutralPanel.transform).GetComponent<InventoryItemUI>();
            m_instances.Add(itemUI.gameObject);
            itemUI.SetUI(m_shopItems[i]);
            neutralPanel.AssignPrice(m_shopItems[i].cost);
            i++;
        }
        foreach (var trinketPanel in m_trinketPanels)
        {
            InventoryItemUI itemUI = Instantiate(m_itemPrefab, trinketPanel.transform).GetComponent<InventoryItemUI>();
            m_instances.Add(itemUI.gameObject);
            itemUI.SetUI(m_shopItems[i]);
            trinketPanel.AssignPrice(m_shopItems[i].cost);
            i++;
        }
        m_instances.Add(Instantiate(m_upgradePrefab, m_upgradePanel.transform));
        m_upgradePanel.AssignPrice(50);
        m_instances.Add(Instantiate(m_removePrefab, m_removePanel.transform));
        m_removePanel.AssignPrice(50);
    }
}
