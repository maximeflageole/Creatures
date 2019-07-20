using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePickState : TurquoiseState
{
    public override void UpdateState()
    {
        
    }

    public override bool VerifyOutConditions()
    {
        //Debug.Log("Creature pick out conditions are true");
        return true;
    }

    public override void StartState()
    {
        Debug.Log("Start Creature Pick State");
        CardEffects.GetInstance().SendCreatureToBattle(Player.GetPlayerInstance().GetCurrentCreature());
    }

    public override void EndState()
    {
        Debug.Log("Exits Creature Pick State");
    }

    public override EBattlePhase GetNextState()
    {
        return EBattlePhase.BattleState;
    }
}
