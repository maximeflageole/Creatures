using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;
using UnityEngine.UI;

public class ItemRewardEvent : TurquoiseEvent
{
    [SerializeField]
    List<EItemTypes> m_itemTypes = new List<EItemTypes>();
    [SerializeField]
    protected EItemRewardType m_itemRewardType;
    [SerializeField]
    protected List<RewardObjectPanel> m_rewardObjectPanel;
    [SerializeField]
    protected GameObject m_unknownRewardPrefab;
    [SerializeField]
    protected GameObject m_itemRewardPrefab;
    [SerializeField]
    protected GameObject m_cardRewardPrefab;
    [SerializeField]
    protected GameObject m_goldRewardPrefab;
    [SerializeField]
    protected Button m_nextButton;
    protected int m_amountToPick;
    protected List<int> m_indexSelected = new List<int>();

    // Start is called before the first frame update
    void BeginReward(EItemRewardType itemRewardType)
    {
        m_itemRewardType = itemRewardType;
        if (m_itemRewardType == EItemRewardType.GetX)
        {

        }
        GenerateRewards();
    }

    public void GenerateRewards()
    {
        int amount = Random.Range(1, 4);
        m_amountToPick = amount;
        m_indexSelected.Clear();
        m_nextButton.interactable = false;
        m_itemTypes.Clear();
        for (int j = 0; j<amount; j++)
        {
            EItemTypes randomType = (EItemTypes)Random.Range(0, (int)EItemTypes.Count);
            m_itemTypes.Add(randomType);
        }
        ResetItemPanels();
        int i = 0;
        foreach (var itemType in m_itemTypes)
        {
            m_rewardObjectPanel[i].gameObject.SetActive(true);
            m_rewardObjectPanel[i].AssignChild(Instantiate(m_unknownRewardPrefab, m_rewardObjectPanel[i].transform));
            i++;
        }
    }

    void ResetItemPanels ()
    {
        foreach (var panel in m_rewardObjectPanel)
        {
            panel.DestroyChild();
            panel.gameObject.SetActive(false);
        }
    }

    public void OnChildClicked(int index)
    {
        if (m_indexSelected.Contains(index))
        {
            return;
        }
        m_indexSelected.Add(index);
        m_rewardObjectPanel[index].DestroyChild();

        switch (m_itemTypes[index])
        {
            case EItemTypes.Gold:
                int goldAmount = Random.Range(10, 50);
                m_rewardObjectPanel[index].AssignChild(Instantiate(m_goldRewardPrefab, m_rewardObjectPanel[index].transform));
                break;
            case EItemTypes.Consumables:
                m_rewardObjectPanel[index].AssignChild(Instantiate(m_itemRewardPrefab, m_rewardObjectPanel[index].transform));
                break;
            case EItemTypes.Neutral:
                m_rewardObjectPanel[index].AssignChild(Instantiate(m_itemRewardPrefab, m_rewardObjectPanel[index].transform));
                break;
            case EItemTypes.TMs:
                m_rewardObjectPanel[index].AssignChild(Instantiate(m_cardRewardPrefab, m_rewardObjectPanel[index].transform));
                break;
            case EItemTypes.Trinkets:
                m_rewardObjectPanel[index].AssignChild(Instantiate(m_itemRewardPrefab, m_rewardObjectPanel[index].transform));
                break;
            default:
                break;
        }
        if (m_indexSelected.Count == m_amountToPick)
        {
            m_nextButton.interactable = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateRewards();
        }
    }
}

namespace Turquoise
{
    public enum EItemRewardType
    {
        PickXfromY,
        GetX,
        Count
    }
}