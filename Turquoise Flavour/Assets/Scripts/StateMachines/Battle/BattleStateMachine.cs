using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    [SerializeField]
    protected EBattlePhase m_currentBattlePhase;
    [SerializeField]
    protected Queue<Creature> m_levelUpsInBattle = new Queue<Creature>();
    [SerializeField]
    protected Dictionary<EBattlePhase, TurquoiseState> BattleStates;
    [SerializeField]
    protected Creature m_playerCreature;
    [SerializeField]
    protected Creature m_enemyCreature;
    [SerializeField]
    protected Creature m_currentCreature;
    public static BattleStateMachine s_battleStateMachine;
    [SerializeField]
    protected int m_turnCount;
    [SerializeField]
    protected bool m_battleEnded;

    // Start is called before the first frame update
    void Start()
    {
        BattleStates = new Dictionary<EBattlePhase, TurquoiseState>();

        CreaturePickState creaturePickState = gameObject.AddComponent(typeof(CreaturePickState)) as CreaturePickState;
        BattleState battleState = gameObject.AddComponent(typeof(BattleState)) as BattleState;
        ExperienceDistributionState experienceDistState = gameObject.AddComponent(typeof(ExperienceDistributionState)) as ExperienceDistributionState;
        BattleRewardState battleRewardState = gameObject.AddComponent(typeof(BattleRewardState)) as BattleRewardState;

        BattleStates.Add(EBattlePhase.CreaturePick, creaturePickState);
        BattleStates.Add(EBattlePhase.BattleState, battleState);
        BattleStates.Add(EBattlePhase.ExperienceDistribution, experienceDistState);
        BattleStates.Add(EBattlePhase.BattleRewards, battleRewardState);
        

        s_battleStateMachine = this;

        StartBattle();
    }

    public Creature GetNextLevelUp()
    {
        if (m_levelUpsInBattle.Count != 0)
        {
            return m_levelUpsInBattle.Dequeue();
        }
        return null;
    }

    public static BattleStateMachine GetInstance()
    {
        return s_battleStateMachine;
    }

    public void StartBattle()
    {
        Debug.Log("Start Battle");
        m_battleEnded = false;
        m_currentBattlePhase = EBattlePhase.CreaturePick;
        BattleStates[m_currentBattlePhase].StartState();
        CardEffects cardEffects = CardEffects.GetInstance();
        cardEffects.Initialization();
        m_turnCount = 1;

        m_playerCreature = cardEffects.GetPlayerCreature();
        m_enemyCreature = cardEffects.GetEnemyCreature();
        m_currentCreature = cardEffects.GetFastestCreature();

        var ai = m_enemyCreature.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.GetNextAction();
        }
    }

    public void EndBattle()
    {
        Debug.Log("End Battle");
        m_playerCreature.QuitBattle();
        m_playerCreature = null;
        m_enemyCreature = null;
        m_battleEnded = true;
        Destroy(CardEffects.GetInstance().gameObject);
        GameMaster.GetInstance().EndCurrentEvent(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_battleEnded)
        {
            return;
        }
        TurquoiseState currentState = BattleStates[m_currentBattlePhase];
        currentState.UpdateState();

        if (currentState.VerifyOutConditions())
        {
            BattleStates[m_currentBattlePhase].EndState();
            m_currentBattlePhase = BattleStates[m_currentBattlePhase].GetNextState();
            if (m_currentBattlePhase == EBattlePhase.None)
            {
                EndBattle();
            }
            else
            {
                BattleStates[m_currentBattlePhase].StartState();
            }
        }
    }

    public bool IsACreatureDead()
    {
        return (m_enemyCreature.IsDead() || m_playerCreature.IsDead());
    }

    public void AddLeveledUpCreature(Creature creature)
    {
        m_levelUpsInBattle.Enqueue(creature);
    }

    public void ChangeTurn()
    {
        m_currentCreature.EndTurn();
        if (m_currentCreature == m_enemyCreature)
        {
            m_currentCreature = m_playerCreature;
            var ai = m_enemyCreature.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.GetNextAction();
            }
        }
        else
        {
            m_currentCreature = m_enemyCreature;
        }
        m_currentCreature.StartTurn();
    }
}

public enum EBattlePhase
{
    CreaturePick,
    BattleState,
    BattleRewards,
    ExperienceDistribution,
    None
}