using System.Collections;
using System.Collections.Generic;
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
    public ETeams m_team = ETeams.None;
    [SerializeField]
    protected Creatures.CreaturesType m_primaryType;
    [SerializeField]
    protected int m_health = 100;
    [SerializeField]
    protected int m_maxHealth = 100;
    [SerializeField]
    protected TextMesh m_healthText;
    [SerializeField]
    protected Deck m_deck;

    public void LoadDeck()
    {
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
                print("Buff" + cardEffect.m_value);
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
    public void Update()
    {
        if (m_healthText != null)
        {
            m_healthText.text = m_health.ToString() + " / " + m_maxHealth.ToString();
        }
    }

    protected void ApplyDamage(int damage)
    {
        m_health -= damage;
    }
}
