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
}
