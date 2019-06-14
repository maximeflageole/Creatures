using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRewardState : TurquoiseState
{
    public override void UpdateState()
    {

    }

    public override bool VerifyOutConditions()
    {
        //Debug.Log("Battle Reward out conditions are true");
        return false;
    }

    public override void EndState()
    {
        Debug.Log("Exits Battle Reward State");
    }
}
