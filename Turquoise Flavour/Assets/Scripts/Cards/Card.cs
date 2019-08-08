using System.Collections;
using System.Collections.Generic;
using Turquoise;
using UnityEngine;

namespace Turquoise
{
    public enum ETarget
    {
        Self,
        Enemy,
        Creature,
        Card,
        None
    }
    public enum ECardGenre
    {
        Attack,
        Power,
        Other
    }
    public enum ERarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        FireCrab,
        None
    }
    public enum ECreaturePart
    {
        Claw,
        Jaw,
        Aquatic,
        Fire,
        Armored,
        None
    }
    public enum ECardEffect
    {
        Damage,
        Healing,
        Draw,
        Discard,
        Exhaust,
        Consume,
        Bleed,
        Armor,
        DamageBuff,
        Find,
        Pick,
        Other,
        Confusion,
        Fear,
        Stun,
        Clears,
        EnergyGain,
        HealingPercent,
        DamagePercent,
        Count
    }

    public enum EDamageType
    {
        Physical,
        Fire,
        Frost,
        Electric,
        Poison,
        Piercing,
        Arcane,
        True,
        None
    }
}

public class Card : MonoBehaviour
{
    public static int FEAR_CHANCES_ON_HUNDRED = 25;

    [SerializeField]
    protected uint m_manaCost = 0;

    public List<SAbilityEffect> m_effects;

    [SerializeField]
    protected CardData m_cardData;

    [SerializeField]
    protected bool m_cardInSelection;
    public bool GetCardInSelection() { return m_cardInSelection; }
    public void SetCardInSelection(bool cardInHand) { m_cardInSelection = cardInHand; } 

    public CardData GetCardData()
    {
        return m_cardData;
    }

    public ETarget GetTarget()
    {
        if (m_cardData != null)
        {
            return m_cardData.targetType;
        }
        return Turquoise.ETarget.None;
    }
    
    // States for card's transform and interaction
    public float scaleSpeed = 1.0f;
    public float targetAngle = 0.0f;
    public Vector3 targetPosition = Vector3.zero;
    public int sortOrder = 0;
    public float offsetAngle = 0.0f;
    public float targetScale = 3.0f;
    public float moveSpeed = 1.0f;
    public float lastOnTime = 0.0f;
    public float curAngle = 0.0f;
    public float nonInteractBegin = 0.0f;
    public float totalDistance = 0.0f;
    public float originHighY = 0.0f;
    public bool isPlaying = false;
    public bool isDropping = false;
    public bool isExhausting = false;
    public bool isConsumed = false;
    public float dropDisplayTime;
    public Dictionary<string, int> info;  // Record card's info here
    public GameObject targetPlayer;       // Record character the card skilled on

    public void SetCardData(CardData cardData)
    {
        m_cardData = cardData;
        var cardUI = GetComponent<CardUI>();
        if (cardUI != null)
        {
            cardUI.InitCardUI(m_cardData.cardName, m_cardData.description, m_cardData.energyCost.ToString(), m_cardData.artwork);
        }
        m_effects = m_cardData.effects;
    }

    public void Reset()
    {
        m_cardInSelection = false;
        isPlaying = false;
        isDropping = false;
        isExhausting = false;
        targetPlayer = null;
        info = null;
        totalDistance = 0.0f;
        GetComponent<TrailRenderer>().enabled = false;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ApplyEffects(Creature selectedCreature, Creature cardPlayingCreature)
    {
        if (!GetIsLucky(m_cardData.chancesOnHundred))
        {
            Debug.Log("Card not played because of chances/100");
            return;
        }
        if (DoesFearTrigger(cardPlayingCreature))
        {
            Debug.Log("Fear triggers. Card skipped");
            return;
        }
        foreach (var cardEffect in m_effects)
        {
            if (!GetIsLucky(cardEffect.m_chancesOfEffectOnHundred))
            {
                continue;
            }
            bool conditionsVerified = true;
            foreach (var condition in cardEffect.m_conditions)
            {
                if (!ConditionHasBoon.VerifyCondition(selectedCreature, condition))
                {
                    conditionsVerified = false;
                    break;
                }
            }
            if (conditionsVerified)
            {
                ApplyEffect(cardEffect, selectedCreature, cardPlayingCreature);
            }
        }
    }

    protected bool DoesFearTrigger(Creature creature)
    {
        var conditionComp = creature.GetConditionsComponent();
        if (conditionComp == null)
        {
            Debug.LogError("Creature has no condition component!");
            return false;
        }
        int fearStacks = conditionComp.GetBoonStacks(ECardEffect.Fear);
        if (fearStacks > 0)
        {
            return GetIsLucky(FEAR_CHANCES_ON_HUNDRED);
        }
        return false;
    }

    protected void ApplyEffect(SAbilityEffect effect, Creature selectedCreature, Creature cardPlayingCreature)
    {
        selectedCreature.ApplyEffect(effect, cardPlayingCreature, m_cardData.damageType);
    }

    bool GetIsLucky(int odds)
    {
        bool chances = (odds == 0 || odds == 100);
        if (!chances)
        {
            int randomNumber = Random.Range(0, 100);
            if (randomNumber < odds)
            {
                chances = true;
            }
        }
        Debug.Log("Is lucky return: " + chances);
        return chances;
    }
}

[System.Serializable]
public struct SAbilityEffect
{
    public ECardEffect m_effect;
    public int m_value;
    public ETarget m_targetType;
    public int m_chancesOfEffectOnHundred;
    public List<ECardEffect> m_conditions;
}