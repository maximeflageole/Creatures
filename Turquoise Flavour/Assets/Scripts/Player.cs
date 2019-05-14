using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_creatures = new List<Creature>();

    [SerializeField]
    protected Creature m_currentCreature;
    public int m_currentCreatureIndex = 0;

    static Player s_playerInstance;

    public static Player GetPlayerInstance()
    {
        if (s_playerInstance == null)
        {
            var player = new GameObject("Player");
            var playerObject = Instantiate(player, Vector3.zero, Quaternion.identity);
            Player playerComp = playerObject.AddComponent<Player>();
            return playerComp;
        }
        return s_playerInstance;
    }

    public void Awake()
    {
        if (s_playerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_playerInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGame()
    {
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
        {
            print("Save file empty");
            return;
        }

        List<CreatureSaveable> creaturesSave = saveData.creaturesSave;
        foreach (var creatureSave in creaturesSave)
        {
            print("Creature load decks");
            var creatureObject = new GameObject("Creature");
            creatureObject.transform.SetParent(transform);
            Creature creature = creatureObject.AddComponent<Creature>();
            creature.CreateFromSave(creatureSave);
            creature.m_team = Turquoise.ETeams.Ally;
            m_creatures.Add(creature);
        }
        if (m_creatures.Count == 0)
        {
            print("Player:Start No creature found");
            return;
        }
        m_currentCreature = m_creatures[0];
        GameMaster.GetInstance().SaveGame();
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

    public void SwapCreature()
    {
        if (m_creatures.Count > 1)
        {
            m_currentCreature.ReturnCreatureFromBattle();
            m_currentCreatureIndex = (m_currentCreatureIndex + 1) % m_creatures.Count;
            m_currentCreature = m_creatures[m_currentCreatureIndex];
            CardEffects.GetCardEffectsInstance().ChangePlayerCreature(m_currentCreature);
        }
    }
}
