using Turquoise;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : MonoBehaviour
{
    [SerializeField]
    protected bool m_inCooldown;
    [SerializeField]
    protected ActiveAbilityData m_abilityData;
    protected int m_currentCooldown;
    protected ActiveAbilityUI m_activeAbilityUI;

    public void LoadAbility(ActiveAbilityData data)
    {
        m_abilityData = data;
    }

    public void LoadAbilityUI(ActiveAbilityUI abilityUI)
    {
        m_activeAbilityUI = abilityUI;
        abilityUI.LoadAbility(m_abilityData);
        abilityUI.SetCooldown(0);
    }

    public ActiveAbilityData GetData()
    {
        return m_abilityData;
    }

    public bool CanCast(int playerMana)
    {
        if (playerMana < m_abilityData.energyCost)
        {
            return false;
        }
        if (m_inCooldown)
        {
            return false;
        }
        return true;
    }

    public ETarget GetTargetType()
    {
        return m_abilityData.targetType;
    }

    public void ApplyEffects(Creature selectedCreature, Creature cardPlayingCreature)
    {
        foreach (var effect in m_abilityData.effects)
        {
            if (effect.m_effect == ECardEffect.Draw)
            {

            }
            else
            {
                ApplyEffect(effect, selectedCreature, cardPlayingCreature);
            }
        }
    }

    protected void ApplyEffect(SAbilityEffect effect, Creature selectedCreature, Creature cardPlayingCreature)
    {
        selectedCreature.ApplyEffect(effect, cardPlayingCreature, selectedCreature);
    }

    public void PlayAbility()
    {
        if (m_abilityData.cooldown > 0)
        {
            m_inCooldown = true;
            m_currentCooldown = m_abilityData.cooldown;
            m_activeAbilityUI.SetCooldown(m_currentCooldown);
        }
    }

    public void StartCreatureTurn()
    {
        if (m_abilityData.repeatable == true)
        {
            if (m_inCooldown)
            {
                m_currentCooldown--;
                m_activeAbilityUI.SetCooldown(m_currentCooldown);
                if (m_currentCooldown <= 0)
                {
                    m_inCooldown = false;
                }
            }
        }
    }
}