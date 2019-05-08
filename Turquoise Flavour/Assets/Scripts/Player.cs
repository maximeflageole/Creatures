﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_creatures = new List<Creature>();

    [SerializeField]
    protected Creature m_currentCreature;

    static Player s_playerInstance;

    public static Player GetPlayerInstance()
    {
        if (s_playerInstance == null)
        {
            var player = new GameObject("Player");
            var playerObject = Instantiate(player, Vector3.zero, Quaternion.identity);
            s_playerInstance = playerObject.AddComponent<Player>();
        }
        return s_playerInstance;
    }

    public void Awake()
    {
        s_playerInstance = this;
        DontDestroyOnLoad(gameObject);
        LoadGame();

        if (m_creatures.Count == 0)
        {
            print("Player:Start No creature found");
            return;
        }
        m_currentCreature = m_creatures[0];
        CardEffects cardEffects = FindObjectOfType<CardEffects>();
        if (cardEffects != null)
        {
            cardEffects.Initialization();
        }
    }

    protected void LoadGame()
    {
        if (SaveSystem.LoadGame() == null)
        {
            print("Save file empty");
            return;
        }
        List<CreatureSaveable> creaturesSave = SaveSystem.LoadGame().creaturesSave;
        foreach (var creatureSave in creaturesSave)
        {
            print("Creature load decks");
            var creatureObject = new GameObject();
            var creatureInstance = Instantiate(creatureObject, transform);
            Creature creature = creatureInstance.AddComponent<Creature>();
            creature.CreateFromSave(creatureSave);
            m_creatures.Add(creature);
        }
        CardEffects cardEffects = FindObjectOfType<CardEffects>();
        if (cardEffects != null)
        {
            cardEffects.m_playerCreature.GetComponent<Creature>().CreateFromSave(creaturesSave[0]);
        }
    }

    public Creature GetCurrentCreature()
    {
        return m_currentCreature;
    }

    public List<Creature> GetCreatures()
    {
        return m_creatures;
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
