using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ConditionHasBoon
{
    public static bool VerifyCondition(Creature target, ECardEffect cardEffect)
    {
        List<ECardEffect> boonsList = target.GetBoons();
        foreach (var boon in boonsList)
        {
            if (boon == cardEffect)
            {
                return true;
            }
        }
        return false;
    }
}
