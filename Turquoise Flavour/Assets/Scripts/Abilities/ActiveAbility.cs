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

    public void LoadAbility(ActiveAbilityData data)
    {
        m_abilityData = data;
    }

    public void LoadAbilityUI(ActiveAbilityUI abilityUI)
    {
        abilityUI.LoadAbility(m_abilityData);
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

    public Turquoise.ETarget GetTargetType()
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

}