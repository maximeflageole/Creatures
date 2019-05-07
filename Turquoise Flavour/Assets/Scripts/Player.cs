using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_creatures;

    [SerializeField]
    protected Creature m_currentCreature;

    static Player s_playerInstance;

    public static Player GetPlayerInstance()
    {
        return s_playerInstance;
    }

    public void Awake()
    {
        s_playerInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        print("PlayerStart");
        CardEffects cardEffects = FindObjectOfType<CardEffects>();
        if (cardEffects == null)
        {
            Debug.Log("CardEffects not found");
        }
        if (m_creatures.Count == 0)
        {
            print("Player:Start No creature found");
            return;
        }
        m_currentCreature = m_creatures[0];
        LoadCreaturesDecks();
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
        return m_currentCreature;
    }

    public bool CanPlayCard(Card card)
    {
        //Verify mana
        if (card.GetCardData().manaCost > m_currentCreature.GetCurrentMana())
        {
            print("Not enough mana to play this card");
            return false;
        }
        //Verify max cards per turn

        //Verify specific conditions

        m_currentCreature.PlayCard(card);
        return true;
    }

    public void TurnEnd()
    {

    }

    public void TurnBegin()
    {
        m_currentCreature.TurnBegin();
    }

    public bool AddCardToCreatureDeck(Cards.ECard card)
    {
        if (m_currentCreature == null)
        {
            print("Player does not have a creature!");
            return false;
        }
        m_currentCreature.AddCardToDeck(card);
        return true;
    }
}
