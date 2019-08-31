using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ItemRewardEvent : TurquoiseEvent
{
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
        List<EItemTypes> itemTypes = new List<EItemTypes>();
        for (int j = 0; j<amount; j++)
        {
            EItemTypes randomType = (EItemTypes)Random.Range(0, (int)EItemTypes.Count);
            itemTypes.Add(randomType);
        }
        ResetItemPanels();
        int i = 0;
        foreach (var itemType in itemTypes)
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
        //Temp


        //EndTemp
        /* TODO: this
        switch (itemType)
        {
            case EItemTypes.Gold:
                break;
            case EItemTypes.Consumables:
                break;
            case EItemTypes.Neutral:
                break;
            case EItemTypes.TMs:
                break;
            case EItemTypes.Trinkets:
                break;
            default:
                break;
        }
        */
        m_rewardObjectPanel[index].DestroyChild();
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