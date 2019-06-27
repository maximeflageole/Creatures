using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ConditionIsBleeding : EffectCondition
{
    public override bool VerifyCondition(GameObject m_target)
    {
        List<ECardEffect> debuffList = m_target.GetComponent<Creature>().GetDebuffs();
        foreach (var debuff in debuffList)
        {
            if (debuff == ECardEffect.Bleed)
            {
                return true;
            }
        }
        return false;
    }
}
