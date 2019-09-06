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

    public void BackButtonPressed()
    {
        ToggleShop();
    }

    public void Awake()
    {
        Reset();
    }

    public void ToggleShop()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        foreach (var obj in m_instances)
        {
            Destroy(obj);
        }
        m_instances.Clear();
        m_shopItems.Clear();
        GenerateContent();
        PopulateShop();
    }

    void ResetGold()
    {
        m_goldTextMesh.text = InventoryManager.GetInstance().GetPlayerGold().ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        ResetGold();
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
            tmPanel.AssignItemAndPrice(m_shopItems[i]);
            tmPanel.SetAvailability(true);
            i++;
        }
        foreach (var consPanel in m_consumablePanels)
        {
            InventoryItemUI itemUI = Instantiate(m_itemPrefab, consPanel.transform).GetComponent<InventoryItemUI>();
            m_instances.Add(itemUI.gameObject);
            itemUI.SetUI(m_shopItems[i]);
            consPanel.AssignItemAndPrice(m_shopItems[i]);
            consPanel.SetAvailability(true);
            i++;
        }
        foreach (var neutralPanel in m_neutralPanels)
        {
            InventoryItemUI itemUI = Instantiate(m_itemPrefab, neutralPanel.transform).GetComponent<InventoryItemUI>();
            m_instances.Add(itemUI.gameObject);
            itemUI.SetUI(m_shopItems[i]);
            neutralPanel.AssignItemAndPrice(m_shopItems[i]);
            neutralPanel.SetAvailability(true);
            i++;
        }
        foreach (var trinketPanel in m_trinketPanels)
        {
            InventoryItemUI itemUI = Instantiate(m_itemPrefab, trinketPanel.transform).GetComponent<InventoryItemUI>();
            m_instances.Add(itemUI.gameObject);
            itemUI.SetUI(m_shopItems[i]);
            trinketPanel.AssignItemAndPrice(m_shopItems[i]);
            trinketPanel.SetAvailability(true);
            i++;
        }
        m_instances.Add(Instantiate(m_upgradePrefab, m_upgradePanel.transform));
        //m_upgradePanel.AssignItemAndPrice(50);
        m_instances.Add(Instantiate(m_removePrefab, m_removePanel.transform));
        //TODO: this
        //m_removePanel.AssignItemAndPrice(50);
    }

    public void OnClick(ShopItemUI childClicked)
    {
        //When an item in the shop has been clicked
        if (InventoryManager.GetInstance().TryBuyItem(childClicked.GetShopItem()))
        {
            childClicked.SetAvailability(false);
            childClicked.GetComponentInChildren<InventoryItemUI>().gameObject.SetActive(false);
        }
    }
}
