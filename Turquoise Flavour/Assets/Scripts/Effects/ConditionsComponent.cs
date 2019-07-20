using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class ConditionsComponent : MonoBehaviour
{
    [SerializeField]
    protected List<Condition> m_boons = new List<Condition>();

    public void TryAddCondition(ConditionData conditionData, int stacks = 1)
    {
        if (IsBuff(conditionData) || IsDebuff(conditionData))
        {
            int i = GetConditionIndex(conditionData);
            if (i != -1)
            {
                m_boons[i].IncrementStacks(stacks);
                return;
            }
            Condition condition = gameObject.AddComponent(typeof(Condition)) as Condition;
            condition.OnCreate(conditionData, stacks);
            m_boons.Add(condition);
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
        for (int i = 0; i< m_boons.Count; i++)
        {
            if (m_boons[i].GetData().cardEffect == data.cardEffect)
            {
                return i;
            }
        }
        return -1;
    }

    public List<ECardEffect> GetBuffs()
    {
        List<ECardEffect> buffs = new List<ECardEffect>();
        foreach (var condition in m_boons)
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
        foreach (var condition in m_boons)
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
            case ECardEffect.DamageBuff:
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
        foreach (var boon in m_boons)
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
        return m_boons;
    }

    public void StartTurn()
    {
        ApplyBoons(EBoonTime.OnTurnStart);
        DecayBoons(EBoonTime.OnTurnStart);
    }

    public void EndTurn()
    {
        ApplyBoons(EBoonTime.OnTurnEnd);
        DecayBoons(EBoonTime.OnTurnEnd);
    }

    void ApplyBoons(EBoonTime effectTime)
    {
        for (int i = 0; i < m_boons.Count; i++)
        {
            var boon = m_boons[i];
            var boonData = boon.GetData();
            if (boonData.boonEffectTime == effectTime)
            {
                SufferBoon(boon);
            }
        }
    }

    void SufferBoon(Condition condition)
    {
        switch (condition.GetData().cardEffect)
        {
            case ECardEffect.Bleed:
                GetComponentInParent<Creature>().ApplyDamage(condition.GetStacks(), EDamageType.True);
                break;
            default:
                break;
        }
    }

    void DecayBoons(EBoonTime decayTime)
    {
        for (int i = 0; i < m_boons.Count; i++)
        {
            var boon = m_boons[i];
            var boonData = boon.GetData();
            if (boonData.boonDecayTime == decayTime)
            {
                switch (boonData.boonDecayType)
                {
                    case EBoonDecayType.All:
                        m_boons.Remove(boon);
                        Destroy(boon);
                        i--;
                        break;
                    case EBoonDecayType.Turn:
                        boon.IncrementStacks(-1);
                        if (boon.GetStacks() <= 0)
                        {
                            m_boons.Remove(boon);
                            Destroy(boon);
                            i--;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void EndBattle()
    {
        for (int i = 0; i<m_boons.Count; i++)
        {
            Destroy(m_boons[i]);
            m_boons.Remove(m_boons[i]);
            i--;
        }
        m_boons.Clear();
    }
}

namespace Turquoise
{
    public enum EBoonDecayType
    {
        None,
        Turn,
        All
    }

    public enum EBoonTime
    {
        None,
        OnTurnStart,
        OnTurnEnd
    }
}
