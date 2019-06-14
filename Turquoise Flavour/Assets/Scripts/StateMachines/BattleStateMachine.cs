using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    [SerializeField]
    protected EBattlePhase m_currentBattlePhase;
    [SerializeField]
    protected List<LevelUpInBattle> m_levelUpsInBattle;
    [SerializeField]
    protected Dictionary<EBattlePhase, TurquoiseState> BattleStates;

    // Start is called before the first frame update
    void Start()
    {
        BattleStates = new Dictionary<EBattlePhase, TurquoiseState>();

        CreaturePickState creaturePickState = gameObject.AddComponent(typeof(CreaturePickState)) as CreaturePickState;
        CardsState cardsState = gameObject.AddComponent(typeof(CardsState)) as CardsState;
        BattleRewardState battleRewardState = gameObject.AddComponent(typeof(BattleRewardState)) as BattleRewardState;

        BattleStates.Add(EBattlePhase.CreaturePickState, creaturePickState);
        BattleStates.Add(EBattlePhase.CardsState, cardsState);
        BattleStates.Add(EBattlePhase.BattleRewards, battleRewardState);

        StartBattle();
    }

    public void StartBattle()
    {
        enabled = true;
        m_currentBattlePhase = EBattlePhase.CreaturePickState;
        BattleStates[m_currentBattlePhase].StartState();
        CardEffects.GetCardEffectsInstance().Initialization();
    }

    public void EndBattle()
    {
        enabled = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        TurquoiseState currentState = BattleStates[m_currentBattlePhase];
        currentState.UpdateState();

        if (currentState.VerifyOutConditions() == true)
        {
            BattleStates[m_currentBattlePhase].EndState();
            m_currentBattlePhase = GetNextState();
            BattleStates[m_currentBattlePhase].StartState();
        }
    }

    EBattlePhase GetNextState()
    {
        switch (m_currentBattlePhase)
        {
            case EBattlePhase.CreaturePickState:
                return EBattlePhase.CardsState;
            case EBattlePhase.CardsState:
                return EBattlePhase.BattleRewards;
            case EBattlePhase.BattleRewards:
                break;
        }
        EndBattle();
        return EBattlePhase.BattleEnd;
    }
}

public enum EBattlePhase
{
    CreaturePickState,
    CardsState,
    BattleRewards,
    BattleEnd
}

public struct LevelUpInBattle
{
    public Creature m_creature;
    public int m_levelGained;
    public LevelUpInBattle(Creature creature, int levelGained)
    {
        m_creature = creature;
        m_levelGained = levelGained;
    }
}