using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_creatures = new List<Creature>();
    [SerializeField]
    protected Explorator m_explorator;
    [SerializeField]
    protected List<EExplorator> m_unlockedExplorators = new List<EExplorator>();

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

    public List<ECard> GetCurrentCreatureDeck()
    {
        return m_currentCreature.GetDeck().m_cards;
    }

    public void Awake()
    {
        if (s_playerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        //Instantiate explorator
        m_explorator = Instantiate(GameMaster.GetInstance().m_exploratorPrefab, transform).GetComponent<Explorator>();
        s_playerInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockExplorator(EExplorator explorator)
    {
        if (!m_unlockedExplorators.Contains(explorator))
        {
            m_unlockedExplorators.Add(explorator);
            SaveSystem.SaveGame();
        }
    }

    public EExplorator GetCurrentExploratorEnum()
    {
        return m_explorator.GetCurrentExplorator();
    }

    public Explorator GetCurrentExplorator()
    {
        return m_explorator;
    }

    public void ChangeExplorator(EExplorator explorator)
    {
        if (ExploratorManager.GetInstance().HasUnlockedExplorator(explorator))
        {
            m_explorator.ChangeExplorator(explorator);
        }
    }

    public void LoadGame()
    {
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
        {
            print("Save file empty");
            return;
        }

        m_unlockedExplorators = saveData.unlockedExplorators;

        if (saveData.currentExplorator == EExplorator.Count)
        {
            saveData.currentExplorator = EExplorator.Biologist;
            m_explorator.ChangeExplorator(saveData.currentExplorator);
        }
        else if (saveData.currentExplorator != m_explorator.GetCurrentExplorator())
        {
            m_explorator.ChangeExplorator(saveData.currentExplorator);
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

    public void EnterOverworld(bool entering)
    {
        m_explorator.GetComponent<SpriteRenderer>().enabled = entering;
        if (entering)
        {
            int i = 0;
            foreach (var creature in m_creatures)
            {
                Overworld.GetInstance().ChangeCreature(i, creature.GetData());
                i++;
            }
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

    public Creature GetCreatureAtIndex(int index)
    {
        if (m_creatures.Count > index)
        {
            return m_creatures[index];
        }
        return null;
    }

    public bool TryPlayCard(Card card)
    {
        //Verify mana
        if (card.GetCardData().energyCost > m_currentCreature.GetCurrentEnergy())
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
        return activeAbility.CanCast(m_currentCreature.GetCurrentEnergy());
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
            creature.transform.SetParent(transform);
            Creature creatureComp = creature.AddComponent<Creature>();
            m_creatures.Add(creatureComp);
            if (m_currentCreature == null)
            {
                m_currentCreature = creatureComp;
            }
            creatureComp.CreateFromCreatureData(creatureData, creatureData.baseDeck, level);
            if (Overworld.HasInstance())
            {
                Overworld.GetInstance().UpdateCreatures();
            }
            SaveSystem.SaveGame();
        }
    }

    public bool HasCreatures()
    {
        return (m_creatures.Count > 0);
    }
}
