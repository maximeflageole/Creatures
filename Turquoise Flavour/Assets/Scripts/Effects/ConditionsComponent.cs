using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ConditionsComponent : MonoBehaviour
{
    [SerializeField]
    protected List<ECardEffect> m_conditions = new List<ECardEffect>();

    public void TryAddCondition(SAbilityEffect cardEffect)
    {
        if (IsBuff(cardEffect.m_effect))
        {
            m_conditions.Add(cardEffect.m_effect);
        }
    }

    public List<ECardEffect> GetBuffs()
    {
        List<ECardEffect> buffs = new List<ECardEffect>();
        foreach (var condition in m_conditions)
        {
            if (IsBuff(condition))
            {
                buffs.Add(condition);
            }
        }
        return buffs;
    }

    public List<ECardEffect> GetDebuffs()
    {
        List<ECardEffect> debuffs = new List<ECardEffect>();
        foreach (var condition in m_conditions)
        {
            if (IsDebuff(condition))
            {
                debuffs.Add(condition);
            }
        }
        return debuffs;
    }

    public static bool IsBuff(ECardEffect cardEffect)
    {
        switch (cardEffect)
        {
            case ECardEffect.Armor:
                return true;
        }
        return false;
    }

    public static bool IsDebuff(ECardEffect cardEffect)
    {
        switch (cardEffect)
        {
            case ECardEffect.Bleed:
                return true;
        }
        return false;
    }

    public int GetArmor()
    {
        foreach (var condition in m_conditions)
        {
            if (condition == ECardEffect.Armor)
            {
                return 1;
            }
        }
        return 0;
    }
}
