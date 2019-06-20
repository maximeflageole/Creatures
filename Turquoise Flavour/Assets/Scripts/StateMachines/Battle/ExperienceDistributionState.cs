using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceDistributionState : TurquoiseState
{
    bool m_isXpDistributed;
    int m_xpAmount;

    public override void UpdateState()
    {
    }

    public override bool VerifyOutConditions()
    {
        return m_isXpDistributed;
    }

    public override void StartState()
    {
        m_xpAmount = 50;
        m_isXpDistributed = false;
        Debug.Log("Start XP Distribution State");
        StartCoroutine("DistributeXP");
    }

    public override void EndState()
    {
        Debug.Log("Exits XP Distribution State");
    }

    protected IEnumerator DistributeXP()
    {
        int iTenth = m_xpAmount / 10;
        for (int i = m_xpAmount; i > 0; i -= iTenth)
        {
            bool hasLeveledUp = Player.GetPlayerInstance().GetCurrentCreature().AddExperience(iTenth);
            if (hasLeveledUp)
            {
                BattleStateMachine.GetInstance().AddLeveledUpCreature(Player.GetPlayerInstance().GetCurrentCreature());
            }
            yield return new WaitForSeconds(0.1f);
        }
        m_isXpDistributed = true;
    }

    protected bool IsBattleFinished()
    {
        return true;
    }

    public override EBattlePhase GetNextState()
    {
        if (IsBattleFinished())
        {
            return EBattlePhase.BattleRewards;
        }
        return EBattlePhase.CreaturePick;
    }
}
