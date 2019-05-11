using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Cards
{
    public enum ETarget
    {
        Self,
        Enemy,
        Creature,
        Card,
        None
    }
    public enum EOwners
    {
        TeamA,
        TeamB,
        Neutral,
        None
    }
    public enum ECardType
    {
        Attack,
        Power,
        Other
    }
    public enum ERarity
    {
        Common,
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
        Buff,
        Debuff,
        Draw,
        Discard,
        Other
    }
}

public class Card : MonoBehaviour
{
    [SerializeField]
    protected uint m_manaCost = 0;

    public List<SCardEffect> m_effects;

    [SerializeField]
    protected EOwners m_owner = EOwners.TeamA;

    [SerializeField]
    protected CardData m_cardData;

    public CardData GetCardData()
    {
        return m_cardData;
    }

    public Cards.ETarget GetTarget()
    {
        if (m_cardData != null)
        {
            return m_cardData.targetType;
        }
        return Cards.ETarget.None;
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
        isPlaying = false;
        isDropping = false;
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
            if (cardEffect.m_effect == ECardEffect.Draw)
            {

            }
            else
            {
                ApplyEffect(cardEffect, selectedCreature);
            }
        }
    }

    protected void ApplyEffect(SCardEffect effect, Creature selectedCreature)
    {
        selectedCreature.ApplyEffect(effect, m_owner);
    }
}

[System.Serializable]
public struct SCardEffect
{
    public ECardEffect m_effect;
    public string m_subtype;
    public int m_value;
    public ETarget m_targetType;
    public float m_chances;
}