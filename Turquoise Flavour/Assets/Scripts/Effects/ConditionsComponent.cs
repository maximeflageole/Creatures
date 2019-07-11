using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ConditionsComponent : MonoBehaviour
{
    [SerializeField]
    protected List<Condition> m_conditions = new List<Condition>();

    public void TryAddCondition(ConditionData conditionData, int stacks = 1)
    {
        if (IsBuff(conditionData) || IsDebuff(conditionData))
        {
            int i = GetConditionIndex(conditionData);
            if (i != -1)
            {
                m_conditions[i].IncrementStacks(stacks);
                return;
            }
            Condition condition = gameObject.AddComponent(typeof(Condition)) as Condition;
            condition.OnCreate(conditionData, stacks);
            m_conditions.Add(condition);
        }
    }

    public void TryAddCondition(SAbilityEffect cardEffect)
    {
        ConditionData conditionData = GameMaster.GetInstance().m_boonList.GetBoonDataFromCardEffect(cardEffect.m_effect);
        if (conditionData != null)
        {
            TryAddCondition(conditionData, cardEffect.m_value);
        }
    }

    public int GetConditionIndex(ConditionData data)
    {
        for (int i = 0; i< m_conditions.Count; i++)
        {
            if (m_conditions[i].GetData().cardEffect == data.cardEffect)
            {
                return i;
            }
        }
        return -1;
    }

    public List<ECardEffect> GetBuffs()
    {
        List<ECardEffect> buffs = new List<ECardEffect>();
        foreach (var condition in m_conditions)
        {
            if (IsBuff(condition.GetData()))
            {
                buffs.Add(condition.GetData().cardEffect);
            }
        }
        return buffs;
    }

    public List<ECardEffect> GetDebuffs()
    {
        List<ECardEffect> debuffs = new List<ECardEffect>();
        foreach (var condition in m_conditions)
        {
            if (IsDebuff(condition.GetData().cardEffect))
            {
                debuffs.Add(condition.GetData().cardEffect);
            }
        }
        return debuffs;
    }

    public List<ECardEffect> GetBoons()
    {
        List < ECardEffect > concat = new List<ECardEffect>();
        concat.AddRange(GetDebuffs());
        concat.AddRange(GetBuffs());
        return concat;
    }

    public static bool IsBuff(ConditionData conditionData)
    {
        return IsBuff(conditionData.cardEffect);
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

    public static bool IsDebuff(ConditionData conditionData)
    {
        return IsDebuff(conditionData.cardEffect);
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

    public int GetBoonStacks(ECardEffect cardEffect)
    {
        foreach (var boon in m_conditions)
        {
            if (boon.GetData().cardEffect == cardEffect)
            {
                return boon.GetStacks();
            }
        }
        return 0;
    }

    public bool HasBoon(ECardEffect cardEffect)
    {
        return (GetBoonStacks(cardEffect) > 0);
    }

    public List<Condition> GetConditions()
    {
        return m_conditions;
    }
}
