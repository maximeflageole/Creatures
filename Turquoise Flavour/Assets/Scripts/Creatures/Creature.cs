﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using Turquoise;
using UnityEngine;
using Creatures;
using Cards;

namespace Turquoise
{
    public enum ETeams
    {
        Enemy,
        Ally,
        None
    }
}

[System.Serializable]
public struct CreatureSaveable
{
    public ECreature m_eCreature;
    public int m_level;
    public List<Cards.ECard> m_deck;
    public int m_currentHealth;
    public int m_experience;


    public CreatureSaveable(ECreature eCreature, int level, Deck deck, int currentHealth, int experience)
    {
        m_eCreature = eCreature;
        m_level = level;
        m_deck = deck.m_cards;
        m_currentHealth = currentHealth;
        m_experience = experience;
    }
}

public class Creature : MonoBehaviour
{
    public static int m_baseMaxMana = 3;

    [SerializeField]
    protected GameObject m_abilityTreePrefab;
    [SerializeField]
    protected ECreature m_eCreature;
    public ETeams m_team = ETeams.None;
    [SerializeField]
    protected ECreatureType m_primaryType;
    [SerializeField]
    protected int m_health = 100;
    [SerializeField]
    protected int m_maxHealth = 100;
    [SerializeField]
    protected int m_armor = 0;
    [SerializeField]
    protected int m_currentMana = 0;
    [SerializeField]
    protected int m_currentMaxMana = m_baseMaxMana;
    [SerializeField]
    protected CreatureUIComp m_creatureUIComp;
    [SerializeField]
    protected Deck m_deck;
    [SerializeField]
    protected Sprite m_sprite;
    [SerializeField]
    protected ActiveAbility m_activeAbility;
    [SerializeField]
    protected CreatureExperience m_experience;
    [SerializeField]
    protected bool m_inBattle;
    [SerializeField]
    protected int m_speed;
    

    public CreatureSaveable GetSaveableCreature()
    {
        return new CreatureSaveable(m_eCreature, m_experience.level, m_deck, m_health, m_experience.experiencePoints);
    }

    public void CreateFromSave(CreatureSaveable creatureSave)
    {
        m_eCreature = creatureSave.m_eCreature;
        m_health = creatureSave.m_currentHealth;
        m_experience.level = creatureSave.m_level;
        m_experience.experiencePoints = creatureSave.m_experience;
        CreatureData creatureData = GameMaster.GetInstance().m_creatureList.GetCreatureDataFromCreatureName(m_eCreature);
        CreateFromCreatureData(creatureData, creatureSave.m_deck, creatureSave.m_level);
    }

    public void CreateFromCreatureData(CreatureData creatureData, List<Cards.ECard> deck, int level = 1, int experience = 0)
    {
        m_experience.levelSpeed = creatureData.levelSpeed;
        m_eCreature = creatureData.eCreature;
        m_maxHealth = creatureData.initialHealth + (creatureData.healthPerLevel * m_experience.level);
        m_health = m_maxHealth;
        m_primaryType = creatureData.creatureType;
        m_currentMaxMana = m_baseMaxMana = creatureData.initialMana;
        m_sprite = creatureData.sprite;
        m_deck.m_cards = deck;
        if (m_activeAbility == null)
        {
            m_activeAbility = gameObject.AddComponent<ActiveAbility>();
            m_activeAbility.LoadAbility(creatureData.activeAbilityData);
        }
    }

    public ActiveAbility GetActiveAbility()
    {
        return m_activeAbility;
    }

    public Deck GetDeck()
    {
        return m_deck;
    }

    private void Awake()
    {
        m_deck = gameObject.AddComponent<Deck>();
        gameObject.AddComponent<ConditionsComponent>();
    }

    public void ApplyEffect(SAbilityEffect cardEffect, Cards.ECardType damageType = Cards.ECardType.None)
    {
        switch(cardEffect.m_effect)
        {
            case Cards.ECardEffect.Buff:
                print("Buff " + cardEffect.m_value + " " + cardEffect.m_subtype);
                var condComponent = GetComponent<ConditionsComponent>();
                if (condComponent != null)
                {
                    condComponent.TryAddBuff(cardEffect.m_subtype, cardEffect.m_value, 1);
                }
                break;
            case Cards.ECardEffect.Damage:
                ApplyDamage(cardEffect.m_value, damageType);
                break;
            case Cards.ECardEffect.Debuff:
                print("Debuff" + cardEffect.m_value);
                break;
            case Cards.ECardEffect.Discard:
                print("Discard" + cardEffect.m_value);
                break;
            case Cards.ECardEffect.Draw:
                print("Draw" + cardEffect.m_value);
                break;
            case Cards.ECardEffect.Healing:
                ApplyDamage(-cardEffect.m_value, damageType);
                break;
            case Cards.ECardEffect.Other:
                print("Other" + cardEffect.m_value);
                break;
            default:
                print("Defaut effect type, not supposed to happen");
                break;
        }
    }

    public void EndBattle()
    {
        ExperienceManager.AddExperience(50, ref m_experience, this);
        m_inBattle = false;
    }

    public void IncrementArmor(int incrementValue)
    {
        m_armor += incrementValue;
    }

    public void Update()
    {
        if (m_inBattle)
        {
            if (m_creatureUIComp != null)
            {
                m_creatureUIComp.UpdateUI(m_health, m_maxHealth, m_armor, m_currentMana, m_currentMaxMana, m_experience.level, m_experience.experiencePoints, ExperienceManager.GetNextLevelXp(m_experience.levelSpeed, m_experience.level));
            }
            if (Input.GetKeyDown("x"))
            {
                ExperienceManager.AddExperience(50, ref m_experience, this);
            }
        }
    }

    protected void ApplyDamage(int damage, ECardType damageType)
    {
        float calculatedDamage = CalculateDamage(damage, damageType);
        int intFinalDamage = Mathf.RoundToInt(calculatedDamage);

        if (m_armor > 0)
        {
            m_armor -= intFinalDamage;
            if (m_armor < 0)
            {
                m_health += m_armor;
                m_armor = 0;
            }
        }
        else
        {
            m_health -= intFinalDamage;
        }
        if (m_health <= 0)
        {
            DieEvent();
        }
    }

    protected float CalculateDamage(int damage, ECardType damageType)
    {
        float typeMultiplier = GetTypeMultiplier(m_primaryType, damageType);
        float calculatedDamage = damage * typeMultiplier;
        calculatedDamage += CalculateArmorPiercingDamage(calculatedDamage, m_armor, damageType);

        return calculatedDamage;
    }

    public static float CalculateArmorPiercingDamage(float initialDamage, int armor, ECardType damageType)
    {
        float additionalDamage = 0.0f;
        if (damageType != ECardType.Piercing)
        {
            return additionalDamage;
        }
        if (initialDamage >= armor/2.0f)
        {
            additionalDamage = armor / 2.0f;
        }
        else if (initialDamage < armor/2.0f)
        {
            additionalDamage = initialDamage;
        }
        return additionalDamage;
    }

    public static float GetTypeMultiplier(ECreatureType creatureType, ECardType damageType)
    {
        switch (creatureType)
        {
            case ECreatureType.Aquatic:
                if (damageType == ECardType.Electric ||
                    damageType == ECardType.Poison)
                {
                    return 2.0f;
                }
                else if (damageType == ECardType.Fire ||
                    damageType == ECardType.Frost)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Automaton:
                if (damageType == ECardType.Piercing)
                {
                    return 2.0f;
                }
                else if (damageType == ECardType.Fire ||
                    damageType == ECardType.Physical)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Beast:
                if (damageType == ECardType.Fire ||
                    damageType == ECardType.Poison)
                {
                    return 2.0f;
                }
                else if (damageType == ECardType.Arcane)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Dark:
                if (damageType == ECardType.Fire ||
                    damageType == ECardType.Electric)
                {
                    return 2.0f;
                }
                else if (damageType == ECardType.Poison ||
                    damageType == ECardType.Arcane)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Electric:
                break;
            case ECreatureType.Ice:
                if (damageType == ECardType.Fire ||
                    damageType == ECardType.Arcane)
                {
                    return 2.0f;
                }
                else if (damageType == ECardType.Frost ||
                    damageType == ECardType.Poison)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Magic:
                break;
            case ECreatureType.Mineral:
                if (damageType == ECardType.Fire ||
                    damageType == ECardType.Electric ||
                    damageType == ECardType.Physical ||
                    damageType == ECardType.Poison)
                {
                    return 0.5f;
                }
                else if (damageType == ECardType.Piercing ||
                        damageType == ECardType.Arcane)
                {
                    return 2.0f;
                }
                break;
            case ECreatureType.Plant:
                if (damageType == ECardType.Fire ||
                    damageType == ECardType.Frost)
                {
                    return 2.0f;
                }
                else if (damageType == ECardType.Poison ||
                        damageType == ECardType.Piercing ||
                        damageType == ECardType.Arcane)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.None:
                break;
            default:
                break;
        }
        return 1.0f;
    }

    public int GetCurrentMana()
    {
        return m_currentMana;
    }

    public void PlayCard(Card card)
    {
        m_currentMana -= card.GetCardData().manaCost;
    }

    public void PlayActiveAbility()
    {
        m_currentMana -= m_activeAbility.GetData().manaCost;
    }

    public void TurnBegin()
    {
        RefreshMana();
        m_armor = 0;
    }

    public void RefreshMana()
    {
        m_currentMana = m_currentMaxMana;
    }

    public void DieEvent()
    {
        CardEffects.GetCardEffectsInstance().DieEvent(this);
    }

    public bool AddCardToDeck(Cards.ECard card)
    {
        m_deck.AddCard(card);
        return true;
    }

    public void SendCreatureToBattle(CreatureUIComp creatureUI)
    {
        m_creatureUIComp = creatureUI;
        m_inBattle = true;
        creatureUI.GetComponent<SpriteRenderer>().sprite = m_sprite;
    }

    public void ReturnCreatureFromBattle()
    {
        m_creatureUIComp = null;
        m_inBattle = false;
    }

    public void OnLevelUp()
    {
        CreatureData creatureData = GameMaster.GetInstance().m_creatureList.GetCreatureDataFromCreatureName(m_eCreature);
        float healthRatio = (float)m_health / (float)m_maxHealth;
        m_maxHealth = creatureData.initialHealth + (creatureData.healthPerLevel * m_experience.level);
        m_health = (int)(m_maxHealth * healthRatio);

        //TODO: Leveling is fucked up right now. Needs work and more importantly, design
        //var abilityTree = Instantiate(m_abilityTreePrefab);
        //abilityTree.GetComponent<AbilityTree>().CreateAbilityTree(GameMaster.GetInstance().m_creatureList.GetCreatureDataFromCreatureName(m_eCreature));
    }

    public int GetSpeed()
    {
        return m_speed;
    }
}

namespace Creatures
{
    [System.Serializable]
    public enum ECreature
    {
        FireCrab,
        Dragon,
        FrostOwl,
        Count
    }
}

