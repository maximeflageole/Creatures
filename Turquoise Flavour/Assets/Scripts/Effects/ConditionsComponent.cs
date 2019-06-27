using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ConditionsComponent : MonoBehaviour
{
    [SerializeField]
    protected List<Condition> m_conditions = new List<Condition>();

    public void TryAddCondition(SAbilityEffect cardEffect, int duration = 1)
    {
        if (IsBuff(cardEffect.m_effect))
        {
            m_conditions.Add(new Condition(cardEffect.m_effect, duration, cardEffect.m_value));
        }
    }

    public List<ECardEffect> GetBuffs()
    {
        List<ECardEffect> buffs = new List<ECardEffect>();
        foreach (var condition in m_conditions)
        {
            if (IsBuff(condition.GetCardEffect()))
            {
                buffs.Add(condition.GetCardEffect());
            }
        }
        return buffs;
    }

    public List<ECardEffect> GetDebuffs()
    {
        List<ECardEffect> debuffs = new List<ECardEffect>();
        foreach (var condition in m_conditions)
        {
            if (IsDebuff(condition.GetCardEffect()))
            {
                debuffs.Add(condition.GetCardEffect());
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
            if (condition.GetCardEffect() == ECardEffect.Armor)
            {
                return condition.GetIntensity();
            }
        }
        return 0;
    }
}
