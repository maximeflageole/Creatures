using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : TurquoiseState
{
    bool m_isBattleStarted = false;

    public override void UpdateState()
    {
    }

    public override bool VerifyOutConditions()
    {
        //Debug.Log("Card state out conditions are true");
        return BattleStateMachine.GetInstance().IsACreatureDead();
    }

    public override void StartState()
    {
        Debug.Log("Start BattleState");
        CardEffects cardEffects = CardEffects.GetInstance();

        if(!m_isBattleStarted)
        {
            m_isBattleStarted = true;
            cardEffects.StartFirstTurn();
        }
    }

    public override void EndState()
    {
        CardEffects.GetInstance().EndBattleState();
        Debug.Log("Exits BattleState");
    }

    public override EBattlePhase GetNextState()
    {
        return EBattlePhase.ExperienceDistribution;
    }
}
