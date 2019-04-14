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
}

public class Card : MonoBehaviour
{
    [SerializeField]
    protected uint m_manaCost = 0;
    [SerializeField]
    protected ETarget m_target = ETarget.None;
    [SerializeField]
    protected List<SCardEffect> m_effects;
    [SerializeField]
    protected EOwners m_owner = EOwners.TeamA;


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

    public void Reset()
    {
        isPlaying = false;
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
            ApplyEffect(cardEffect, selectedCreature);
        }
    }

    protected void ApplyEffect(SCardEffect effect, Creature selectedCreature)
    {
        selectedCreature.ApplyEffect(effect, m_owner);
    }
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

[System.Serializable]
public struct SCardEffect
{
    public ECardEffect m_effect;
    public string m_subtype;
    public int m_value;
    public ETarget m_targetType;
}
