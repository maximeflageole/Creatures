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

    public void TryAddCondition(SAbilityEffect abilityEffect)
    {
        ConditionData conditionData = GameMaster.GetInstance().m_boonList.GetBoonDataFromCardEffect(abilityEffect.m_effect);
        if (conditionData != null)
        {
            TryAddCondition(conditionData, abilityEffect.m_value);
        }
    }

    public void TryAddCondition(ECardEffect cardEffect, int stacks = 1)
    {
        if (stacks != 0)
        {
            ConditionData conditionData = GameMaster.GetInstance().m_boonList.GetBoonDataFromCardEffect(cardEffect);
            if (conditionData != null)
            {
                TryAddCondition(conditionData, stacks);
            }
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
            case ECardEffect.Strength:
            case ECardEffect.HeAttacks:
            case ECardEffect.HeProtects:
            case ECardEffect.BleedingAttacks:
            case ECardEffect.UrgencyArmor:
            case ECardEffect.Charge:
            case ECardEffect.Cleanse:
            case ECardEffect.Fast:
            case ECardEffect.Haste:
            case ECardEffect.ManaSurge:
            case ECardEffect.Preparation:
            case ECardEffect.Recycle:
            case ECardEffect.SignatureMove:
            case ECardEffect.Vigor:
            case ECardEffect.Ethereal:
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
            case ECardEffect.Stun:
            case ECardEffect.Fear:
            case ECardEffect.Confusion:
            case ECardEffect.Charge:
            case ECardEffect.ForgottenMove:
            case ECardEffect.ManaSink:
            case ECardEffect.Slow:
            case ECardEffect.Unprepared:
            case ECardEffect.Burn:
            case ECardEffect.Vulnerable:
            case ECardEffect.Weakened:
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
                TriggerBoon(boon);
            }
        }
    }

    public int GetCalculatedDamage(int initialDamage)
    {
        int calculatedDamage = initialDamage;
        foreach (var boon in m_boons)
        {
            if (boon.GetData().cardEffect == ECardEffect.Strength)
            {
                calculatedDamage += boon.GetStacks();
            }
        }
        // This is not a mistake, we want to apply damage buffs and debuffs before multipliers
        foreach (var boon in m_boons)
        {
            if (boon.GetData().cardEffect == ECardEffect.Stun)
            {
                calculatedDamage = (int)((float)calculatedDamage * 0.66);
            }
        }
        Debug.Log("Calculated damage: " + calculatedDamage);
        return calculatedDamage;
    }

    void TriggerBoon(Condition condition)
    {
        switch (condition.GetData().cardEffect)
        {
            case ECardEffect.Bleed:
                GetComponentInParent<Creature>().ApplyDamage(condition.GetStacks(), EDamageType.True);
                break;
            case ECardEffect.UrgencyArmor:
                if (GetBoonStacks(ECardEffect.Armor) == 0)
                {
                    TryAddCondition(ECardEffect.Armor, 5 * condition.GetStacks());
                }
                break;
            case ECardEffect.Vigor:
                GetComponentInParent<Creature>().HealPercent(condition.GetStacks());
                break;
            case ECardEffect.Cleanse:
                ClearDebuffs(condition.GetStacks());
                break;
            case ECardEffect.Burn:
                GetComponentInParent<Creature>().ApplyDamage(condition.GetStacks(), EDamageType.True);
                break;
            default:
                break;
        }
    }

    public void RemoveConditionStack(ECardEffect effect, int qty = 1)
    {
        Condition condition = GetCondition(effect);
        if (condition != null)
        {
            condition.IncrementStacks(-qty);
            VerifyBoon(condition);
        }
    }

    public void RemoveAllConditionStacks(ECardEffect effect)
    {
        Condition condition = GetCondition(effect);
        if (condition != null)
        {
            condition.RemoveAllStacks();
            VerifyBoon(condition);
        }
    }

    Condition GetCondition(ECardEffect effect)
    {
        foreach (var boon in m_boons)
        {
            if (boon.GetData().cardEffect == effect)
            {
                return boon;
            }
        }
        return null;
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
                        if (VerifyBoon(boon))
                        {
                            i--;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    bool VerifyBoon(Condition boon)
    {
        if (boon.GetStacks() < 1)
        {
            m_boons.Remove(boon);
            Destroy(boon);
            return true;
        }
        return false;
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

    public void ClearDebuffs(int clearAmount)
    {
        int debuffsStacks = 0;
        foreach (var boon in m_boons)
        {
            if (IsDebuff(boon.GetData()))
            {
                debuffsStacks += boon.GetStacks();
            }
        }

        for (int i = 0; i < debuffsStacks && i < clearAmount; i++)
        {
            int randNumber = Random.Range(0, debuffsStacks);
            for (int j = 0; j < m_boons.Count; j++)
            {
                Condition boon = m_boons[j];
                if (IsDebuff(boon.GetData()))
                {
                    randNumber -= boon.GetStacks();
                    if (randNumber < 0)
                    {
                        boon.IncrementStacks(-1);
                        if (boon.GetStacks() <= 0)
                        {
                            m_boons.Remove(boon);
                            Destroy(boon);
                        }
                        debuffsStacks--;
                        clearAmount--;
                        i--;
                        break;
                    }
                }
            }
        }
    }

    public bool VerifyComplexCondition(sComplexConditions sComplexConditions)
    {
        int first = GetBoonStacks(sComplexConditions.condition);
        int second = sComplexConditions.comparativeNumber;
        if (sComplexConditions.otherCondition != ECardEffect.Count)
        {
            second = GetBoonStacks(sComplexConditions.otherCondition);
        }
        switch (sComplexConditions.operation)
        {
            case EOperation.Equals:
                return first == second;
            case EOperation.NotEqual:
                return first != second;
            case EOperation.Bigger:
                return first > second;
            case EOperation.BiggerOrEqual:
                return first >= second;
            case EOperation.Less:
                return first < second;
            case EOperation.LessOrEqual:
                return first <= second;
        }
        return false;
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
