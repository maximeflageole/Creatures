using System.Collections;
using System.Collections.Generic;
using Turquoise;
using UnityEngine;

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
    [SerializeField]
    protected ETeams m_team = ETeams.None;
    [SerializeField]
    protected Creatures.CreaturesType m_primaryType;
    [SerializeField]
    protected float m_health = 100.0f;
    [SerializeField]
    protected float m_maxHealth = 100.0f;

    public void ApplyEffect(SCardEffect cardEffect, Cards.EOwners owner)
    {
        switch(cardEffect.m_effect)
        {
            case ECardEffect.Buff:
                print("Buff" + cardEffect.m_value);
                break;
            case ECardEffect.Damage:
                print("Damage " + cardEffect.m_value);
                break;
            case ECardEffect.Debuff:
                print("Debuff" + cardEffect.m_value);
                break;
            case ECardEffect.Discard:
                print("Discard" + cardEffect.m_value);
                break;
            case ECardEffect.Draw:
                print("Draw" + cardEffect.m_value);
                break;
            case ECardEffect.Healing:
                print("Healing" + cardEffect.m_value);
                break;
            case ECardEffect.Other:
                print("Other" + cardEffect.m_value);
                break;
            default:
                print("Defaut effect type, not supposed to happen");
                break;
        }
    }
}
