using System.Collections;
using System.Collections.Generic;
using TMPro;
using Turquoise;
using UnityEngine;
using UnityEngine.UI;

namespace Turquoise
{
    public enum ETeams
    {
        Player,
        Enemy,
        Wild,
        Ally,
        None
    }
}

public class Creature : MonoBehaviour
{
    public static int m_baseMaxMana = 3;

    public ETeams m_team = ETeams.None;
    [SerializeField]
    protected Creatures.CreaturesType m_primaryType;
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
    protected TextMeshPro m_manaTextMesh;
    [SerializeField]
    protected TextMesh m_healthText;
    [SerializeField]
    protected TextMesh m_armorText;
    [SerializeField]
    protected TextMesh m_ConditionsText;
    [SerializeField]
    protected Deck m_deck;

    public void LoadDeck()
    {
        if (m_deck == null)
        {
            m_deck = gameObject.AddComponent(typeof(Deck)) as Deck;
        }
        m_deck.LoadGame();
    }

    public Deck GetDeck()
    {
        return m_deck;
    }

    public void ApplyEffect(SCardEffect cardEffect, Cards.EOwners owner)
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
                ApplyDamage(cardEffect.m_value);
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
                ApplyDamage(-cardEffect.m_value);
                break;
            case Cards.ECardEffect.Other:
                print("Other" + cardEffect.m_value);
                break;
            default:
                print("Defaut effect type, not supposed to happen");
                break;
        }
    }

    public void IncrementArmor(int incrementValue)
    {
        m_armor += incrementValue;
    }

    public void Update()
    {
        if (m_healthText != null)
        {
            m_healthText.text = m_health.ToString() + " / " + m_maxHealth.ToString();
        }
        if (m_armorText != null)
        {
            m_armorText.gameObject.SetActive(m_armor != 0);
            m_armorText.text = m_armor.ToString();
        }
        if (m_manaTextMesh != null)
        {
            m_manaTextMesh.text = "Mana: " + m_currentMana + "/" + m_baseMaxMana;
        }
    }

    protected void ApplyDamage(int damage)
    {
        if (m_armor > 0)
        {
            m_armor -= damage;
            if (m_armor < 0)
            {
                m_health += m_armor;
                m_armor = 0;
            }
        }
        else
        {
            m_health -= damage;
        }
        if (m_health <= 0)
        {
            DieEvent();
        }
    }

    public int GetCurrentMana()
    {
        return m_currentMana;
    }

    public void PlayCard(Card card)
    {
        m_currentMana -= card.GetCardData().manaCost;
    }

    public void TurnBegin()
    {
        m_currentMana = m_currentMaxMana;
        m_armor = 0;
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
}
