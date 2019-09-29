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
        GiveCreatureCurrency();
        m_rewardChosen = false;
        PickNextCreature();
        if (m_levelingCreature == null)
        {
            m_rewardChosen = true;
            return;
        }
        GameObject reward = Instantiate(GameMaster.GetInstance().GetRewardPrefab());
        reward.GetComponent<RewardEvent>().SetCallback(RewardPickedCallback);
        reward.GetComponent<RewardEvent>().SetRewardedCreature(m_levelingCreature);
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

    void GiveCreatureCurrency()
    {
        var enemyCreature = CardEffects.GetInstance().GetEnemyCreature().GetData().eCreature;
        var playerCreature = CardEffects.GetInstance().GetPlayerCreature().GetData().eCreature;

        InventoryManager.GetInstance().AddCreatureCurrency(enemyCreature, 3);
        InventoryManager.GetInstance().AddCreatureCurrency(playerCreature, 3);
    }
}
