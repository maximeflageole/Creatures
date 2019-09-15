using System.Collections;
using System.Collections.Generic;
using TMPro;
using Turquoise;
using UnityEngine;
using Creatures;

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
    public List<ECard> m_deck;
    public int m_currentHealth;
    public int m_experience;
    public EItem m_item;


    public CreatureSaveable(ECreature eCreature, int level, Deck deck, int currentHealth, int experience, EItem item)
    {
        m_eCreature = eCreature;
        m_level = level;
        m_deck = deck.m_cards;
        m_currentHealth = currentHealth;
        m_experience = experience;
        m_item = item;
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
    protected int m_currentEnergy = 0;
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
    [SerializeField]
    protected CreatureData m_creatureData;
    public CreatureData GetData() { return m_creatureData; }
    [SerializeField]
    protected ConditionsComponent m_conditionsComponent;
    public ConditionsComponent GetConditionsComponent() { return m_conditionsComponent; }
    [SerializeField]
    protected EItem m_trinket = EItem.Count;

    public CreatureSaveable GetSaveableCreature()
    {
        return new CreatureSaveable(m_eCreature, m_experience.level, m_deck, m_health, m_experience.experiencePoints, m_trinket);
    }

    public void CreateFromSave(CreatureSaveable creatureSave)
    {
        m_eCreature = creatureSave.m_eCreature;
        m_health = creatureSave.m_currentHealth;
        m_experience.level = creatureSave.m_level;
        m_experience.experiencePoints = creatureSave.m_experience;
        m_creatureData = GameMaster.GetInstance().m_creatureList.GetCreatureDataFromCreatureName(m_eCreature);
        CreateFromCreatureData(m_creatureData, creatureSave.m_deck, creatureSave.m_level);
        m_trinket = creatureSave.m_item;
    }

    public void CreateFromCreatureData(CreatureData creatureData, List<ECard> deck, int level = 1, int experience = 0)
    {
        m_experience.levelSpeed = creatureData.levelSpeed;
        m_eCreature = creatureData.eCreature;
        m_maxHealth = creatureData.initialHealth + (creatureData.healthPerLevel * m_experience.level);
        m_health = m_maxHealth;
        m_primaryType = creatureData.creatureType;
        m_baseMaxMana = creatureData.initialMana;
        m_sprite = creatureData.sprite;
        m_deck.m_cards = deck;
        m_creatureData = creatureData;
        if (m_activeAbility == null)
        {
            m_activeAbility = gameObject.AddComponent<ActiveAbility>();
            m_activeAbility.LoadAbility(creatureData.activeAbilityData);
        }
    }

    public string GetName()
    {
        return m_creatureData.creatureName;
    }

    public Sprite GetSprite()
    {
        return m_creatureData.sprite;
    }

    public ActiveAbility GetActiveAbility()
    {
        return m_activeAbility;
    }

    public Deck GetDeck()
    {
        return m_deck;
    }

    public float GetHealthRatio()
    {
        return (float)m_health / (float)m_maxHealth;
    }

    private void Awake()
    {
        m_deck = gameObject.AddComponent<Deck>();
        m_conditionsComponent = gameObject.AddComponent<ConditionsComponent>();
    }

    public void ApplyEffect(SAbilityEffect cardEffect, Creature cardPlayingCreature, Creature selectedCreature, EDamageType damageType = EDamageType.None)
    {
        if (m_conditionsComponent == null)
        {
            Debug.LogError("Creature has no conditions component!");
            return;
        }

        Creature targetCreature = selectedCreature;
        //Determine target
        switch (cardEffect.m_targetType)
        {
            case ETarget.Self:
                targetCreature = cardPlayingCreature;
                break;
            default:
                break;
        }
        targetCreature.m_conditionsComponent.TryAddCondition(cardEffect);

        switch (cardEffect.m_effect)
        {
            case ECardEffect.Damage:
                int calculatedDamage = cardPlayingCreature.m_conditionsComponent.GetCalculatedDamage(cardEffect.m_value);
                ApplyDamage(calculatedDamage, damageType);
                m_conditionsComponent.TryAddCondition(ECardEffect.Bleed, cardPlayingCreature.m_conditionsComponent.GetBoonStacks(ECardEffect.BleedingAttacks));
                break;
            case ECardEffect.Healing:
                ApplyDamage(-cardEffect.m_value, damageType);
                break;
            case ECardEffect.Clears:
                m_conditionsComponent.ClearDebuffs(cardEffect.m_value);
                break;
            case ECardEffect.EnergyGain:
                IncrementEnergy(cardEffect.m_value);
                break;
            case ECardEffect.HealingPercent:
                ApplyDamagePercent(-cardEffect.m_value);
                break;
            case ECardEffect.DamagePercent:
                ApplyDamagePercent(cardEffect.m_value);
                break;
            case ECardEffect.VampiricHeal:
                cardPlayingCreature.ApplyDamage(-cardEffect.m_value * selectedCreature.m_conditionsComponent.GetBoonStacks(ECardEffect.Bleed));
                break;
            case ECardEffect.HeAttacks:
                cardPlayingCreature.m_conditionsComponent.RemoveBuff(ECardEffect.HeProtects, 1);
                break;
            case ECardEffect.HeProtects:
                cardPlayingCreature.m_conditionsComponent.RemoveBuff(ECardEffect.HeAttacks, 1);
                break;
            default:
                break;
        }
    }

    public List<ECardEffect> GetBuffs()
    {
        if (m_conditionsComponent != null)
        {
            return m_conditionsComponent.GetBuffs();
        }
        return null;
    }

    public List<ECardEffect> GetDebuffs()
    {
        if (m_conditionsComponent != null)
        {
            return m_conditionsComponent.GetDebuffs();
        }
        return null;
    }

    public List<ECardEffect> GetBoons()
    {
        if (m_conditionsComponent != null)
        {
            return m_conditionsComponent.GetBoons();
        }
        return null;
    }

    public void EndBattle()
    {
        if (m_conditionsComponent != null)
        {
            m_conditionsComponent.EndBattle();
        }
    }

    public void QuitBattle()
    {
        m_inBattle = false;
    }

    public bool AddExperience(int Amount)
    {
        int level = m_experience.level;
        ExperienceManager.AddExperience(Amount, ref m_experience, this);
        return (level != m_experience.level);
    }

    public List<Condition> GetConditions()
    {
        if (m_conditionsComponent != null)
        {
            return m_conditionsComponent.GetConditions();
        }
        return null;
    }

    public void Update()
    {
        if (m_inBattle)
        {
            if (m_creatureUIComp != null)
            {
                m_creatureUIComp.UpdateUI(m_health, m_maxHealth, GetCurrentEnergy(), GetCurrentMaxMana(), m_experience.level, m_experience.experiencePoints, ExperienceManager.GetNextLevelXp(m_experience.levelSpeed, m_experience.level), m_conditionsComponent.GetConditions());
            }
        }
    }

    public int GetCurrentMaxMana()
    {
        int returnInt = m_baseMaxMana;
        returnInt += m_conditionsComponent.GetBoonStacks(ECardEffect.ManaSurge);
        returnInt -= m_conditionsComponent.GetBoonStacks(ECardEffect.ManaSink);
        return returnInt;
    }

    public void PlayAnimation(ECardGenre eCardGenre)
    {
        m_creatureUIComp.PlayAnimation(eCardGenre);
    }

    public void ApplyDamage(int damage, EDamageType damageType = EDamageType.None)
    {
        float calculatedDamage = CalculateDamage(damage, damageType);
        int intFinalDamage = Mathf.RoundToInt(calculatedDamage);

        EDamageIntensity damageIntensity = EDamageIntensity.Normal;
        float typeMultiplier = GetTypeMultiplier(m_primaryType, damageType);

        if (damage <0)
        {
            damageIntensity = EDamageIntensity.Heal;
        }
        else if (typeMultiplier > 1)
        {
            damageIntensity = EDamageIntensity.SuperEffective;
        }
        else if (typeMultiplier < 1)
        {
            damageIntensity = EDamageIntensity.Reduced;
        }
        m_creatureUIComp.ReceiveDamage(intFinalDamage, damageIntensity);

        if (m_armor > 0 && damageType != EDamageType.True && intFinalDamage>0)
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
        if (m_health > m_maxHealth)
        {
            m_health = m_maxHealth;
        }
    }

    public void ApplyDamagePercent(int damagePercent)
    {
        float damagePercentFloat = (float)damagePercent;
        damagePercentFloat /= 100.0f;
        float calculatedDamage = damagePercentFloat * m_maxHealth;

        m_health -= (int)calculatedDamage;
        m_health = Mathf.Clamp(m_health, 0, m_maxHealth);
    }

    public void StartTurn()
    {
        if (m_conditionsComponent != null)
        {
            m_conditionsComponent.StartTurn();
        }
    }

    public void EndTurn()
    {
        if (m_conditionsComponent != null)
        {
            m_conditionsComponent.EndTurn();
        }
    }

    protected float CalculateDamage(int damage, EDamageType damageType)
    {
        //TODO: CalculateDamage seems bugged AF. Seems like a good idea to treat everything as floats until the very end
        float typeMultiplier = GetTypeMultiplier(m_primaryType, damageType);
        float calculatedDamage = damage * typeMultiplier;
        calculatedDamage += CalculateArmorPiercingDamage(calculatedDamage, m_armor, damageType);

        return calculatedDamage;
    }

    public static float CalculateArmorPiercingDamage(float initialDamage, int armor, EDamageType damageType)
    {
        float additionalDamage = 0.0f;
        if (damageType != EDamageType.Piercing)
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

    public static float GetTypeMultiplier(ECreatureType creatureType, EDamageType damageType)
    {
        switch (creatureType)
        {
            case ECreatureType.Aquatic:
                if (damageType == EDamageType.Electric ||
                    damageType == EDamageType.Poison)
                {
                    return 2.0f;
                }
                else if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Frost)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Automaton:
                if (damageType == EDamageType.Piercing)
                {
                    return 2.0f;
                }
                else if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Physical)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Beast:
                if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Poison)
                {
                    return 2.0f;
                }
                else if (damageType == EDamageType.Arcane)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Dark:
                if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Electric)
                {
                    return 2.0f;
                }
                else if (damageType == EDamageType.Poison ||
                    damageType == EDamageType.Arcane)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Electric:
                break;
            case ECreatureType.Ice:
                if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Arcane)
                {
                    return 2.0f;
                }
                else if (damageType == EDamageType.Frost ||
                    damageType == EDamageType.Poison)
                {
                    return 0.5f;
                }
                break;
            case ECreatureType.Magic:
                break;
            case ECreatureType.Mineral:
                if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Electric ||
                    damageType == EDamageType.Physical ||
                    damageType == EDamageType.Poison)
                {
                    return 0.5f;
                }
                else if (damageType == EDamageType.Piercing ||
                        damageType == EDamageType.Arcane)
                {
                    return 2.0f;
                }
                break;
            case ECreatureType.Plant:
                if (damageType == EDamageType.Fire ||
                    damageType == EDamageType.Frost)
                {
                    return 2.0f;
                }
                else if (damageType == EDamageType.Poison ||
                        damageType == EDamageType.Piercing ||
                        damageType == EDamageType.Arcane)
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

    public int GetCurrentEnergy()
    {
        return m_currentEnergy;
    }

    public void PlayCard(Card card)
    {
        m_currentEnergy -= card.GetCardData().energyCost;
    }

    public void IncrementEnergy(int increment)
    {
        m_currentEnergy += increment;
    }

    public void PlayActiveAbility()
    {
        m_currentEnergy -= m_activeAbility.GetData().energyCost;
    }

    public void TurnBegin()
    {
        RefreshEnergy();
        m_armor = 0;
    }

    public void RefreshEnergy()
    {
        m_currentEnergy = GetCurrentMaxMana();
    }

    public void HealPercent(float percent)
    {
        float amountToHeal = (m_maxHealth * percent)/100.0f;
        Heal((int)amountToHeal);
    }

    public void Heal(int healAmount)
    {
        m_health += healAmount;
        Mathf.Clamp(m_health, 0, m_maxHealth);
    }

    public void DieEvent()
    {
        Debug.Log("Creature " + m_team.ToString() + " died");
    }

    public bool IsDead()
    {
        return m_health <= 0;
    }

    public void InstantKill()
    {
        m_health = 0;
    }

    public bool AddCardToDeck(ECard card)
    {
        m_deck.AddCard(card);
        return true;
    }

    public bool RemoveCardFromDeck(ECard card)
    {
        return m_deck.RemoveCard(card);
    }

    public void SendCreatureToBattle(CreatureUIComp creatureUI)
    {
        m_creatureUIComp = creatureUI;
        m_inBattle = true;
        creatureUI.GetComponent<SpriteRenderer>().sprite = m_sprite;
        ApplyTrinketEffects();
    }

    void ApplyTrinketEffects()
    {
        if (m_trinket != EItem.Count)
        {
            var trinketData = InventoryManager.GetInstance().GetItemFromEnum(m_trinket);
            if (trinketData.effects.Count != 0)
            {
                foreach(var effect in trinketData.effects)
                {
                    m_conditionsComponent.TryAddCondition(effect);
                }
            }
        }
    }

    public int GetDrawCardModifier()
    {
        int returnInt = 0;
        returnInt += m_conditionsComponent.GetBoonStacks(ECardEffect.Preparation);
        returnInt -= m_conditionsComponent.GetBoonStacks(ECardEffect.Unprepared);
        return returnInt;
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

    public ERewardType GetNextLevelUpReward()
    {
        return m_creatureData.abilityTree.abilities[m_experience.level];
    }

    public List<ECard> GetNextLevelUpCards(ERarity rarity, int amount)
    {
        return m_creatureData.abilityTree.GetCardsByRarity(rarity, amount, true);
    }

    public void EquipItem(EItem item)
    {
        if (m_trinket != EItem.Count)
        {
            InventoryManager.GetInstance().AddInventoryItemFromEItem(m_trinket);
        }
        m_trinket = item;
        InventoryManager.GetInstance().AddInventoryItemFromEItem(item, -1);
    }

    public void UseItem(InventoryItemData itemData)
    {
        foreach (var effect in itemData.effects)
        {
            switch (effect.m_effect)
            {
                case ECardEffect.Healing:
                    ApplyDamage(-effect.m_value);
                    break;
                case ECardEffect.HealingPercent:
                    ApplyDamagePercent(-effect.m_value);
                    break;
            }
        }
        InventoryManager.GetInstance().RemoveInventoryItem(itemData, 1);
    }

    public void GiveTM(InventoryItemData itemData)
    {
        foreach (var card in itemData.cardsTms)
        {
            AddCardToDeck(card);
        }
        InventoryManager.GetInstance().RemoveInventoryItem(itemData, 1);
    }
}

namespace Creatures
{
    [System.Serializable]
    public enum ECreature
    {
        FireCrab,                   // 0
        Dragon,
        FrostOwl,
        Doggo,
        Count
    }
}

