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
        Other,
        Count
    }

    public enum ECardType
    {
        Physical,
        Fire,
        Frost,
        Electric,
        Poison,
        Piercing,
        Arcane,
        None
    }
}

public class Card : MonoBehaviour
{
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
    public float dropDisplayTime;
    public Dictionary<string, int> info;  // Record card's info here
    public GameObject targetPlayer;       // Record character the card skilled on

    public void SetCardData(CardData cardData)
    {
        m_cardData = cardData;
        var cardUI = GetComponent<CardUI>();
        if (cardUI != null)
        {
            cardUI.InitCardUI(m_cardData.cardName, m_cardData.description, m_cardData.manaCost.ToString(), m_cardData.artwork);
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

    public void ApplyEffects(Creature selectedCreature)
    {
        foreach (var cardEffect in m_effects)
        {
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
                ApplyEffect(cardEffect, selectedCreature);
            }
        }
    }

    protected void ApplyEffect(SAbilityEffect effect, Creature selectedCreature)
    {
        selectedCreature.ApplyEffect(effect, m_cardData.damageType);
    }
}

[System.Serializable]
public struct SAbilityEffect
{
    public ECardEffect m_effect;
    public int m_value;
    public ETarget m_targetType;
    public List<ECardEffect> m_conditions;
}