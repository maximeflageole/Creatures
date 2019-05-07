using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_creatures;

    [SerializeField]
    protected int creatureIndex;

    public void Start()
    {
        print("PlayerStart");
        CardEffects cardEffects = FindObjectOfType<CardEffects>();
        if (cardEffects == null)
        {
            Debug.Log("CardEffects not found");
        }

        cardEffects.Initialization(this);
    }

    protected void LoadCreaturesDecks()
    {
        foreach (var creature in m_creatures)
        {
            print("Creature load decks");
            creature.LoadDeck();
        }
    }

    public Creature GetCurrentCreature()
    {
        if (m_creatures.Count >= creatureIndex)
        {
            return m_creatures[creatureIndex];
        }
        return null;
    }

    public bool CanPlayCard(Card card)
    {
        //Verify mana
        if (card.GetCardData().manaCost > m_creatures[0].GetCurrentMana())
        {
            print("Not enough mana to play this card");
            return false;
        }
        //Verify max cards per turn

        //Verify specific conditions

        m_creatures[0].PlayCard(card);
        return true;
    }

    public void TurnEnd()
    {

    }

    public void TurnBegin()
    {
        m_creatures[0].TurnBegin();
    }
}
