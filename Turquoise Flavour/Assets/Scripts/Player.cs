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

    public const int MAX_CREATURE_COUNT = 3;

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
    }

    public Creature GetCurrentCreature()
    {
        return m_currentCreature;
    }

    public List<Creature> GetCreatures()
    {
        return m_creatures;
    }

    public bool TryPlayCard(Card card)
    {
        //Verify mana
        if (card.GetCardData().energyCost > m_currentCreature.GetCurrentMana())
        {
            print("Not enough mana to play this card");
            return false;
        }
        //Verify max cards per turn

        //Verify specific conditions

        m_currentCreature.PlayCard(card);
        return true;
    }

    public bool CanPlayActiveAbility()
    {
        ActiveAbility activeAbility = m_currentCreature.GetActiveAbility();
        return activeAbility.CanCast(m_currentCreature.GetCurrentMana());
    }

    public void PlayActiveAbility()
    {
        m_currentCreature.PlayActiveAbility();
        Debug.Log("Play Active");
    }

    public void TurnEnd()
    {

    }

    public void TurnBegin()
    {
        m_currentCreature.TurnBegin();
    }

    public bool AddCardToCreatureDeck(Turquoise.ECard card)
    {
        if (m_currentCreature == null)
        {
            print("Player does not have a creature!");
            return false;
        }
        m_currentCreature.AddCardToDeck(card);
        return true;
    }

    public void SwapCreature(Creature creature)
    {
        m_currentCreature.ReturnCreatureFromBattle();
        m_currentCreature = creature;
        CardEffects.GetInstance().ChangePlayerCreature(m_currentCreature);
    }

    public void CaptureCreature(CreatureData creatureData, int level)
    {
        if (m_creatures.Count < MAX_CREATURE_COUNT)
        {
            var creature = new GameObject("Creature");
            var creatureObject = Instantiate(creature, transform);
            Creature creatureComp = creatureObject.AddComponent<Creature>();
            m_creatures.Add(creatureComp);
            if (m_currentCreature == null)
            {
                m_currentCreature = creatureComp;
            }
            creatureComp.CreateFromCreatureData(creatureData, creatureData.baseDeck, level);
        }
    }

    public bool HasCreatures()
    {
        return (m_creatures.Count > 0);
    }
}
