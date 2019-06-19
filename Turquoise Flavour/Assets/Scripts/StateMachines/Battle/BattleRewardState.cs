using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRewardState : TurquoiseState
{
    [SerializeField]
    protected bool m_rewardChosen;
    [SerializeField]
    protected Creature m_levelingCreature;

    public override void UpdateState()
    {

    }

    public override bool VerifyOutConditions()
    {
        return m_rewardChosen;
    }

    public override void StartState()
    {
        m_rewardChosen = false;
        PickNextCreature();
        if (m_levelingCreature == null)
        {
            m_rewardChosen = true;
            return;
        }
        GameObject reward = Instantiate(GameMaster.GetInstance().GetRewardPrefab());
        reward.GetComponent<RewardEvent>().SetCallback(RewardPickedCallback);
        Debug.Log("Starts Battle Reward State");
    }

    public override void EndState()
    {
        Debug.Log("Exits Battle Reward State");
    }

    public override EBattlePhase GetNextState()
    {
        return EBattlePhase.None;
    }

    public void RewardPickedCallback()
    {
        PickNextCreature();
        if (m_levelingCreature == null)
        {
            m_rewardChosen = true;
        }
        else
        {
            GameObject reward = Instantiate(GameMaster.GetInstance().GetRewardPrefab());
            reward.GetComponent<RewardEvent>().SetCallback(RewardPickedCallback);
        }
    }

    protected void PickNextCreature()
    {
        m_levelingCreature = BattleStateMachine.GetInstance().GetNextLevelUp();
        if (m_levelingCreature != null)
        {
            return;
        }
    }
}
